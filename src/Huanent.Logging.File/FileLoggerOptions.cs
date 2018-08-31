using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Huanent.Logging.File
{
    public class FileLoggerOptions
    {
        public Func<string, LogLevel, bool> Filter { get; set; }

        public string Path { get; set; }
    }
}