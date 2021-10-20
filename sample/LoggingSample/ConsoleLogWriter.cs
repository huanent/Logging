using Huanent.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
