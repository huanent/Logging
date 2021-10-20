using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text;

namespace Huanent.Logging.File
{
    public class FileLoggerWriter : ILoggerWriter
    {
        readonly FileLogOptions _options;
        readonly string _prefixPath;

        public FileLoggerWriter(IOptions<FileLogOptions> options)
        {
            _options = options.Value;
            _prefixPath = Path.Combine(AppContext.BaseDirectory, _options.Path);
            if (!Directory.Exists(_prefixPath)) Directory.CreateDirectory(_prefixPath);
        }

        public void WriteLog(LogLevel level, string message, string name, Exception exception, EventId eventId)
        {
            var fileName = $"{DateTimeOffset.UtcNow:yyyMMdd}.txt";
            var path = Path.Combine(_prefixPath, fileName);
            var logBuilder = new StringBuilder();
            var spliter = $"---------------[{name}] [{level}] [{eventId}] [{DateTimeOffset.UtcNow}]---------------";
            logBuilder.AppendLine(spliter);
            logBuilder.AppendLine(message);
            if (exception != default) logBuilder.AppendLine(exception.ToString());
            logBuilder.AppendLine(spliter);
            logBuilder.AppendLine(Environment.NewLine);
            System.IO.File.AppendAllText(path, logBuilder.ToString());
        }
    }
}
