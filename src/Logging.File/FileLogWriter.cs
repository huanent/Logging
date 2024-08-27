using Huanent.Logging.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Huanent.Logging.File;

public class FileLogWriter : ILogWriter
{
    private readonly FileLoggerOptions options;
    private readonly string prefixPath;
    private readonly static ConcurrentQueue<Log> queue = new();
    private static uint writing = 0;

    record Log(string FilePath, LogLevel Level, string Message, string Name, Exception Exception, EventId EventId);

    public FileLogWriter(IOptions<FileLoggerOptions> options)
    {
        this.options = options.Value;
        prefixPath = Path.Combine(AppContext.BaseDirectory, this.options.Path);
    }

    public void WriteLog(LogLevel level, string message, string name, Exception exception, EventId eventId)
    {
        var fileName = $"{DateTimeOffset.UtcNow.ToString(options.DateFormat)}.txt";
        var path = Path.Combine(prefixPath, fileName);
        queue.Enqueue(new Log(path, level, message, name, exception, eventId));

        if (Interlocked.Exchange(ref writing, 1) == 0)
        {
            _ = WriteLogAsync();
        }
    }

    private static async Task WriteLogAsync()
    {
        while (queue.TryDequeue(out var log))
        {
            var logBuilder = new StringBuilder();
            var splitter = $"[{log.Level}] [{log.Name}] [{log.EventId}] [{DateTimeOffset.UtcNow}]";
            logBuilder.AppendLine(splitter);
            logBuilder.AppendLine(log.Message);
            if (log.Exception != default) logBuilder.AppendLine(log.Exception.ToString());
            logBuilder.AppendLine();
            var folder = Path.GetDirectoryName(log.FilePath);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            await System.IO.File.AppendAllTextAsync(log.FilePath, logBuilder.ToString());
        }

        Interlocked.Exchange(ref writing, 0);
    }
}

