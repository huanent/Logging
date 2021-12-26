using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;

namespace Huanent.Logging.Sqlite
{
    public class SqliteLoggerRepository
    {
        private readonly SqliteLoggerOptions _options;
        private readonly string _connectionString;
        private readonly string _tableName = "Log";

        public SqliteLoggerRepository(IOptions<SqliteLoggerOptions> options)
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

        internal void Insert(SqliteLog log)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Execute(
                    @"insert into Log (Level, Message, Name, Exception, DateTime) values (@Level,@Message, @Name, @Exception, @DateTime)",
                    new
                    {
                        Level = (int) log.Level,
                        log.Message,
                        log.Name,
                        Exception = log.Exception,
                        DateTime = log.DateTime.ToUnixTimeMilliseconds()
                    });
            }
        }

        internal void EnsureLogTableCreated()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Execute($@"
create table if not exists ""{_tableName}""
(
    Id        integer
    constraint Log_pk
        primary key autoincrement,
    Level     integer not null,
    Message   text,
    Name      text,
    Exception text,
    DateTime integer not null
)");
            }
        }

        public SqliteLogQueryResult Query(SqliteLogQueryParams @params)
        {
            var sqlBuilder = new SqlBuilder();
            var countSql = sqlBuilder.AddTemplate($@"select count(1) from ""{_tableName}"" /**where**/");

            var listSql = sqlBuilder.AddTemplate(
                $@"select * from ""{_tableName}"" /**where**/ limit {@params.PageSize} offset {@params.SkipCount}"
            );

            sqlBuilder.Where($"DateTime >= {@params.StartDateTime.ToUnixTimeMilliseconds()}");
            sqlBuilder.Where($"DateTime <= {@params.EndDateTime.ToUnixTimeMilliseconds()}");
            if (@params.Level.HasValue) sqlBuilder.Where("Level = @Level");
            if (!string.IsNullOrWhiteSpace(@params.Name)) sqlBuilder.Where("Name = @Name");

            if (!string.IsNullOrWhiteSpace(@params.Keyword))
            {
                sqlBuilder.Where("Message like '%@Keyword%'  or Exception like '%@Keyword%'");
            }

            var result = new SqliteLogQueryResult
            {
                PageIndex = @params.PageIndex,
                PageSize = @params.PageSize
            };

            using (var connection = new SqliteConnection(_connectionString))
            {
                result.LogCount = connection.QueryFirstOrDefault<long>(countSql.RawSql, @params);
                var dynamicList = connection.Query(listSql.RawSql, @params);
                
                result.Logs = dynamicList
                    .Select(s => new SqliteLog(s.Level, s.Message, s.Name, s.Exception, s.DateTime))
                    .ToArray();
            }

            return result;
        }
    }
}