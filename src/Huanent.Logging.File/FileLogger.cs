using Huanent.Logging.File;
using System;

namespace Microsoft.Extensions.Logging.File
{
    public partial class FileLogger : ILogger
    {
        private readonly FileLoggerWriter _fileLoggerWriter;
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly string _name;

        public FileLogger(string name, FileLoggerWriter fileLoggerWriter)
        {
            _name = string.IsNullOrEmpty(name) ? nameof(FileLogger) : name;
            _filter = fileLoggerWriter.Options.Filter ?? ((category, logLevel) => true);
            _fileLoggerWriter = fileLoggerWriter;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NoopDisposable.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _filter(_name, logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            string message = formatter(state, exception);

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            _fileLoggerWriter.WriteLine(logLevel, message, _name, exception);
        }

        private class NoopDisposable : IDisposable
        {
            public static NoopDisposable Instance = new NoopDisposable();

            public void Dispose()
            {
            }
        }
    }
}