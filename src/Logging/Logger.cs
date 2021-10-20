using Microsoft.Extensions.Logging;
using System;

namespace Huanent.Logging
{
    public partial class Logger : ILogger
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly string _name;
        private readonly ILoggerWriter _loggerWriter;

        public Logger(string name, Func<string, LogLevel, bool> filter, ILoggerWriter loggerWriter)
        {
            _name = string.IsNullOrWhiteSpace(name) ? nameof(Logger) : name;
            _filter = filter ?? ((category, logLevel) => true);
            _loggerWriter = loggerWriter;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NoopDisposable.Instance;
        }

        public bool IsEnabled(LogLevel logLevel) => _filter(_name, logLevel);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            string message = formatter(state, exception);
            if (string.IsNullOrEmpty(message)) return;
            _loggerWriter.WriteLog(logLevel, message, _name, exception, eventId);
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
