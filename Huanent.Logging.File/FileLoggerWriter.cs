
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Logging.File
{
    public class FileLoggerWriter
    {
        static object _locker = new object();
        ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();
        public CancellationTokenSource CancellationToken { get; } = new CancellationTokenSource();
        string _logDir = "logs";


        public static FileLoggerWriter _fileLoggerWriter;

        public FileLoggerWriter()
        {
            Task.Run(() =>
            {
                CreateLogDir();
                var logBuilder = new StringBuilder();
                while (!CancellationToken.IsCancellationRequested)
                {
                    logBuilder.Clear();
                    string date = DateTime.Now.ToString("yyyyMMdd");
                    int nowCount = _queue.Count;

                    if (nowCount == 0)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    int count = nowCount > 10 ? 10 : nowCount;

                    for (int i = 0; i < count; i++)
                    {
                        _queue.TryDequeue(out string log);
                        logBuilder.Append(log);
                    }

                    string logs = logBuilder.ToString();

                    try
                    {
                        WriteLog(date, logs);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        CreateLogDir();
                        WriteLog(date, logs);
                    }
                    catch (Exception)
                    {

                    }
                }
            });
        }

        private void WriteLog(string date, string log)
        {
            System.IO.File.AppendAllText($"{_logDir}/{date}.txt", log);
        }

        /// <summary>
        /// µ¥ÀýFileLoggerWriter
        /// </summary>
        public static FileLoggerWriter Instance
        {
            get
            {
                lock (_locker)
                {
                    if (_fileLoggerWriter == null)
                    {
                        lock (_locker)
                        {
                            _fileLoggerWriter = _fileLoggerWriter ?? new FileLoggerWriter();
                        }
                    }
                    return _fileLoggerWriter;
                }
            }
        }

        public void WriteLine(LogLevel level, string message, string name, Exception exception)
        {
            var logBuilder = new StringBuilder();

            logBuilder.AppendLine($"-----{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}-----");
            logBuilder.AppendLine($"{level.ToString()}:{name}");
            logBuilder.AppendLine(message);
            if (exception != null) logBuilder.AppendLine(exception.ToString());
            logBuilder.AppendLine("-----End-----");
            logBuilder.AppendLine();

            _queue.Enqueue(logBuilder.ToString());
        }

        void CreateLogDir()
        {
            if (!Directory.Exists(_logDir)) Directory.CreateDirectory(_logDir);
        }
    }
}
