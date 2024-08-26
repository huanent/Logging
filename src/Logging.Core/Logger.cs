using Microsoft.Extensions.Logging;
using System;

namespace Huanent.Logging.Core;

public class Logger(string name, Func<string, LogLevel, bool> filter, ILogWriter loggerWriter) : ILogger
{
    private readonly Func<string, LogLevel, bool> filter = filter ?? ((category, logLevel) => true);
    private readonly string name = string.IsNullOrWhiteSpace(name) ? nameof(Logger) : name;

    public IDisposable BeginScope<TState>(TState state) => NoopDisposable.Instance;

    public bool IsEnabled(LogLevel logLevel) => filter(name, logLevel);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;
        ArgumentNullException.ThrowIfNull(formatter);
        string message = formatter(state, exception);
        if (string.IsNullOrEmpty(message)) return;
        loggerWriter.WriteLog(logLevel, message, name, exception, eventId);
    }

    private class NoopDisposable : IDisposable
    {
        public static NoopDisposable Instance = new();

        public void Dispose()
        {
        }
    }
}

