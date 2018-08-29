using Huanent.Logging.File;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Logging.File
{
    [ProviderAlias("File")]
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly FileLoggerWriter _fileLoggerWriter;
        private readonly FileLoggerOptions _options = new FileLoggerOptions();

        public FileLoggerProvider(FileLoggerOptions options)
        {
            _options = options;
            _fileLoggerWriter = new FileLoggerWriter(options);
        }

        public ILogger CreateLogger(string name)
        {
            return new FileLogger(name, _fileLoggerWriter);
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