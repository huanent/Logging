using Microsoft.Extensions.Logging;
using System;

namespace Huanent.Logging.Sqlite
{
    public class SqliteLoggerOptions
    {
        public Func<string, LogLevel, bool> Filter { get; set; }

        public string Path { get; set; }
    }
}
