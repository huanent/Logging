using Huanent.Logging.Core;
using Microsoft.Extensions.Logging;
using System;

namespace Huanent.Logging.Sqlite
{
    public class SqliteLogWriter : ILogWriter
    {
        private readonly SqliteLoggerRepository _loggerRepository;

        public SqliteLogWriter(SqliteLoggerRepository loggerRepository)
        {
            _loggerRepository = loggerRepository;
        }

        public void WriteLog(LogLevel level, string message, string name, Exception exception, EventId eventId)
        {
            _loggerRepository.Insert(new SqliteLog(level, message, name, exception));
        }
    }
}