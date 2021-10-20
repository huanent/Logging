using Huanent.Logging.Core;
using Microsoft.Extensions.Logging;
using System;

namespace LoggingSample
{
    public class ConsoleLogWriter : ILogWriter
    {
        public void WriteLog(LogLevel level, string message, string name, Exception exception, EventId eventId)
        {
            Console.WriteLine($"From {nameof(ConsoleLogWriter)} {level} {message} {name} {exception} {eventId}");
        }
    }
}
