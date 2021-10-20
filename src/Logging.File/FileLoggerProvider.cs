using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Huanent.Logging.File
{
    [ProviderAlias("File")]
    public class FileLoggerProvider : LoggerProvider
    {
        public FileLoggerProvider(ILoggerWriter loggerWriter) : base(loggerWriter)
        {
        }
    }
}
