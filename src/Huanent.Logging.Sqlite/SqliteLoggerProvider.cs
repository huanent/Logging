using Huanent.Logging.Sqlite;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Logging
{
    [ProviderAlias("Sqlite")]
    public class SqliteLoggerProvider : ILoggerProvider
    {
        private readonly SqliteLoggerWriter _fileLoggerWriter;
        private readonly SqliteLoggerOptions _options = new SqliteLoggerOptions();

        public SqliteLoggerProvider(SqliteLoggerOptions options)
        {
            _options = options;
            _fileLoggerWriter = new SqliteLoggerWriter(options);
        }

        public ILogger CreateLogger(string name)
        {
            return new SqliteLogger(name, _fileLoggerWriter);
        }

        public void Dispose()
        {
            _fileLoggerWriter.CancellationToken.Cancel();
            while (!_fileLoggerWriter.LogWriteDone)
            {
                Task.Delay(100).Wait();
            }
        }
    }
}
