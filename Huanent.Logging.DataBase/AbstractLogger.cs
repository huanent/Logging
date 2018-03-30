using Huanent.Logging.Abstract;
using System;

namespace Microsoft.Extensions.Logging.Abstract
{

    public partial class AbstractLogger : ILogger
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly string _name;
        private readonly ILoggerWriter _loggerWriter;

        public AbstractLogger(string name, Func<string, LogLevel, bool> filter, ILoggerWriter loggerWriter)
        {
            _name = string.IsNullOrEmpty(name) ? nameof(AbstractLogger) : name;
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
