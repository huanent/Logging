using Dapper;
using Huanent.Logging.Core;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace Huanent.Logging.Sqlite
{
    public class SqliteLogWriter : ILogWriter
    {
        private readonly SqliteLoggerOptions _options;
        private readonly string _connectionString;

        public SqliteLogWriter(IOptions<SqliteLoggerOptions> options)
        {
            _options = options.Value;
            var path = Path.Combine(AppContext.BaseDirectory, _options.Path);
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory ?? throw new ArgumentNullException());

            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = path,
                Pooling = true
            };

            _connectionString = builder.ConnectionString;
            EnsureLogTableCreated();
        }

        public void WriteLog(LogLevel level, string message, string name, Exception exception, EventId eventId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Execute(
                    @"insert into Log (Level, Message, Name, Exception, Timestamp) values (@Level,@Message, @Name, @Exception, @Timestamp)",
                    new SqliteLog(level, message, name, exception));
            }
        }

        private void EnsureLogTableCreated()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Execute(@"
create table if not exists Log
(
    Level     integer not null,
    Message   text,
    Name      text,
    Exception text,
    Timestamp integer not null
)");
            }
        }
    }
}