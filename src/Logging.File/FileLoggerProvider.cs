using Huanent.Logging.Core;
using Microsoft.Extensions.Logging;

namespace Huanent.Logging.File
{
    [ProviderAlias("File")]
    public class FileLoggerProvider : LoggerProvider
    {
        public FileLoggerProvider(ILogWriter loggerWriter) : base(loggerWriter)
        {
        }
    }
}
