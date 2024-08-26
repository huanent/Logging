using Microsoft.Extensions.Logging;
using System;

namespace Huanent.Logging.Core
{
    [ProviderAlias("Implementation")]
    public class LoggerProvider(ILogWriter loggerWriter) : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> filter;
        private readonly ILogWriter _loggerWriter = loggerWriter;

        public LoggerProvider(Func<string, LogLevel, bool> filter, ILogWriter loggerWriter)
            : this(loggerWriter)
        {
            this.filter = filter;
        }

        public ILogger CreateLogger(string name)
        {
            return new Logger(name, filter, _loggerWriter);
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
