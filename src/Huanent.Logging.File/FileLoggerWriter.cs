using Huanent.Logging.File;
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
        private readonly string _logDir;
        private ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();

        public FileLoggerWriter(FileLoggerOptions options)
        {
            Options = options;
            _logDir = Options.Path ?? Path.Combine(AppContext.BaseDirectory, "logs");
            BeginAsyncQueueWriter();
        }

        public CancellationTokenSource CancellationToken => new CancellationTokenSource();

        public bool LogWriteDone => _queue.Count == 0;

        public FileLoggerOptions Options { get; }

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

        private void BeginAsyncQueueWriter()
        {
            Task.Run(() =>
            {
                CreateLogDir();
                var logBuilder = new StringBuilder();
                while (!CancellationToken.IsCancellationRequested || _queue.Count > 0)
                {
                    logBuilder.Clear();
                    string date = DateTime.Now.ToString("yyyyMMdd");
                    int nowCount = _queue.Count;

                    if (nowCount == 0)
                    {
                        Thread.Sleep(50);
                        continue;
                    }

                    nowCount = nowCount > 30 ? 30 : nowCount;

                    for (int i = 0; i < nowCount; i++)
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

        private void CreateLogDir()
        {
            if (!Directory.Exists(_logDir)) Directory.CreateDirectory(_logDir);
        }

        private void WriteLog(string date, string log)
        {
            System.IO.File.AppendAllText(Path.Combine(_logDir, $"{date}.txt"), log);
        }
    }
}