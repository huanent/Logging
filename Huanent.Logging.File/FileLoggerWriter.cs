
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
                var list = new List<string>();
                while (!CancellationToken.IsCancellationRequested)
                {
                    string date = DateTime.Now.ToString("yyyyMMdd");
                    if (_queue.TryDequeue(out string log))
                    {
                        try
                        {
                            WriteLog(date, log);
                        }
                        catch (DirectoryNotFoundException)
                        {
                            CreateLogDir();
                            WriteLog(date, log);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
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
