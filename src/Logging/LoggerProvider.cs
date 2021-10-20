using Microsoft.Extensions.Logging;
using System;

namespace Huanent.Logging
{
    [ProviderAlias("Implementation")]
    public class LoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;
        readonly ILoggerWriter _loggerWriter;

        public LoggerProvider(ILoggerWriter loggerWriter)
        {
            _loggerWriter = loggerWriter;
        }

        public LoggerProvider(Func<string, LogLevel, bool> filter, ILoggerWriter loggerWriter)
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
