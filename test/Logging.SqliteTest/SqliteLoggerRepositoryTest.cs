using Dapper;
using Huanent.Logging.Sqlite;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Linq;

namespace Logging.SqliteTest;

[TestClass]
public class SqliteLoggerRepositoryTest
{
    private readonly string _dbDir = Path.Combine(AppContext.BaseDirectory, "test");

    public SqliteLoggerRepositoryTest()
    {
        if (!Directory.Exists(_dbDir)) return;
        Directory.Delete(_dbDir, true);
        Directory.CreateDirectory(_dbDir);
    }

    public IOptions<SqliteLoggerOptions> GetOptions(string name)
    {
        var mock = new Mock<IOptions<SqliteLoggerOptions>>();

        mock.Setup(s => s.Value).Returns(new SqliteLoggerOptions
        {
            Path = Path.Combine(_dbDir, $"{name}.db")
        });

        return mock.Object;
    }

    [TestMethod]
    public void EnsureLogTableCreatedTest()
    {
        var options = GetOptions(nameof(EnsureLogTableCreatedTest));
        _ = new SqliteLoggerRepository(options);
        Assert.IsTrue(File.Exists(options.Value.Path));
        using var connection = new SqliteConnection($"Data Source='{options.Value.Path}'");
        var tables = connection.Query<string>("select  name from sqlite_master");
        Assert.IsTrue(tables.Contains("Log"));
    }

    [TestMethod]
    public void InsertTest()
    {
        var options = GetOptions(nameof(InsertTest));
        var repository = new SqliteLoggerRepository(options);
        repository.Insert(new SqliteLog(LogLevel.Debug, "message", "name", new Exception()));
    }

    [TestMethod]
    public void Query_All_Test()
    {
        var options = GetOptions(nameof(Query_All_Test));
        var repository = new SqliteLoggerRepository(options);
        repository.Insert(new SqliteLog(LogLevel.Debug, "message", "name", new Exception()));
        repository.Insert(new SqliteLog(LogLevel.Error, "error", null, null));
        var result = repository.Query(new SqliteLogQueryParams());
        Assert.AreEqual(result.LogCount, 2);
    }
}