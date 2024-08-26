using Huanent.Logging.Core;
using Microsoft.Extensions.Logging;

namespace Huanent.Logging.File;

[ProviderAlias("File")]
public class FileLoggerProvider(ILogWriter loggerWriter) : LoggerProvider(loggerWriter)
{
}

