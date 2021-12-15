using Microsoft.Extensions.Logging;
using System;

namespace Huanent.Logging.Sqlite
{
    public class SqliteLog
    {
        public SqliteLog(LogLevel level, string message, string name, Exception exception)
        {
            Level = (int) level;
            Message = message;
            Name = name;
            Exception = exception?.ToString();
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public int Level { get; }
        public string Message { get; }
        public string Name { get; }
        public string Exception { get; }
        public long Timestamp { get; }
    }
}