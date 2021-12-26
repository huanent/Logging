using Microsoft.Extensions.Logging;
using System;

namespace Huanent.Logging.Sqlite
{
    public class SqliteLog
    {
        public SqliteLog(LogLevel level, string message, string name, Exception exception)
        {
            Level = level;
            Message = message;
            Name = name;
            Exception = exception?.ToString();
            DateTime = DateTimeOffset.UtcNow;
        }

        internal SqliteLog(long level, string message, string name, string exception, long dateTime)
        {
            Level = (LogLevel) level;
            Message = message;
            Name = name;
            Exception = exception;
            DateTime = DateTimeOffset.Now;
        }

        public LogLevel Level { get; }
        public string Message { get; }
        public string Name { get; }
        public string Exception { get; }
        public DateTimeOffset DateTime { get; }
    }
}