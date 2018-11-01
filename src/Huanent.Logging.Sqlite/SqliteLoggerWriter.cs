using Huanent.Logging.Sqlite;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Logging
{
    public class SqliteLoggerWriter
    {
        public string _currentDate;
        private readonly SQLiteConnection _dbConnection;
        private readonly string _logDir;
        private ConcurrentQueue<LogModel> _queue = new ConcurrentQueue<LogModel>();

        public SqliteLoggerWriter(SqliteLoggerOptions options)
        {
            Options = options;
            _logDir = Options.Path ?? Path.Combine(AppContext.BaseDirectory, "logs");
            _dbConnection = new SQLiteConnection();
            BeginAsyncQueueWriter();
        }

        public CancellationTokenSource CancellationToken => new CancellationTokenSource();

        public bool LogWriteDone => _queue.Count == 0;

        public SqliteLoggerOptions Options { get; }

        public void WriteLine(LogLevel level, string message, string name, Exception exception)
        {
            _queue.Enqueue(new LogModel
            {
                Category = name ?? string.Empty,
                DateTime = DateTime.Now,
                Exception = exception?.ToString() ?? string.Empty,
                LogLevel = level.ToString() ?? string.Empty,
                Massage = message ?? string.Empty
            });
        }

        private void BeginAsyncQueueWriter()
        {
            Task.Run(() =>
            {
                CreateLogDir();
                var appendLogs = new List<LogModel>();
                while (!CancellationToken.IsCancellationRequested || _queue.Count > 0)
                {
                    appendLogs.Clear();
                    string date = DateTime.Now.ToString("yyyyMMdd");
                    int nowCount = _queue.Count;

                    if (nowCount == 0)
                    {
                        Thread.Sleep(50);
                        continue;
                    }

                    nowCount = nowCount > 30 ? 30 : nowCount;

                    for (int i = 0; i < nowCount; i++)
                    {
                        _queue.TryDequeue(out var log);
                        appendLogs.Add(log);
                    }

                    try
                    {
                        WriteLog(date, appendLogs);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        CreateLogDir();
                        WriteLog(date, appendLogs);
                    }
                    catch (Exception)
                    {
                    }
                }
            });
        }

        private void CreateLogDir()
        {
            if (!Directory.Exists(_logDir)) Directory.CreateDirectory(_logDir);
        }

        private void CreateTable()
        {
            var cmd = _dbConnection.CreateCommand();
            cmd.CommandText = @"
 CREATE TABLE IF NOT EXISTS Logs
(
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Category TEXT NOT NULL,
DateTime DATETIME NOT NULL,
Exception TEXT NOT NULL,
LogLevel TEXT NOT NULL,
Massage TEXT NOT NULL
);
";
            cmd.ExecuteNonQuery();
        }

        private void InsertLogs(List<LogModel> logs)
        {
            var cmd = _dbConnection.CreateCommand();
            var sqlbuilder = new StringBuilder();
            for (int i = 0; i < logs.Count(); i++)
            {
                string row = i.ToString();
                string sql = $"INSERT INTO Logs VALUES (null,@Category{row},@DateTime{row},@Exception{row},@LogLevel{row},@Massage{row});";
                sqlbuilder.AppendLine(sql);
                cmd.Parameters.AddRange(new[] {
                    new SQLiteParameter($"@Category{row}", logs[i].Category),
                    new SQLiteParameter($"@DateTime{row}",logs[i].DateTime),
                    new SQLiteParameter($"@Exception{row}", logs[i].Exception),
                    new SQLiteParameter($"@LogLevel{row}", logs[i].LogLevel),
                    new SQLiteParameter($"@Massage{row}", logs[i].Massage),
                });
            }
            cmd.CommandText = sqlbuilder.ToString();
            cmd.ExecuteNonQuery();
        }

        private void WriteLog(string date, List<LogModel> logs)
        {
            string dbPath = Path.Combine(_logDir, $"{date}.db");
            _dbConnection.ConnectionString = $"Data Source={dbPath}";
            _dbConnection.Open();
            if (_currentDate != date)
            {
                CreateTable();
                _currentDate = date;
            }
            InsertLogs(logs);
            _dbConnection.Close();
        }
    }
}
