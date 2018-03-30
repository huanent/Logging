using Huanent.Logging.Abstract;
using System;

namespace Microsoft.Extensions.Logging.Abstract
{
    [ProviderAlias("Abstract")]
    public class AbstractLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly ILoggerWriter _loggerWriter;

        public AbstractLoggerProvider(ILoggerWriter loggerWriter)
        {
            _loggerWriter = loggerWriter;
        }

        public AbstractLoggerProvider(Func<string, LogLevel, bool> filter, ILoggerWriter loggerWriter)
            : this(loggerWriter)
        {
            _filter = filter;
        }

        public ILogger CreateLogger(string name) => new AbstractLogger(name, _filter, _loggerWriter);

        public void Dispose()
        {
            _loggerWriter.Dispose();
        }
    }
}
