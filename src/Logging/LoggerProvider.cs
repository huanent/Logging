using Microsoft.Extensions.Logging;
using System;

namespace Huanent.Logging
{
    [ProviderAlias("Implementation")]
    public class LoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly ILogWriter _loggerWriter;

        public LoggerProvider(ILogWriter loggerWriter)
        {
            _loggerWriter = loggerWriter;
        }

        public LoggerProvider(Func<string, LogLevel, bool> filter, ILogWriter loggerWriter)
            : this(loggerWriter)
        {
            _filter = filter;
        }

        public ILogger CreateLogger(string name)
        {
            return new Logger(name, _filter, _loggerWriter);
        }

        public void Dispose()
        {
            if (_loggerWriter is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
