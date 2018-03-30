using Huanent.Logging.Abstract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sample
{
    public class MyLogWriter : ILoggerWriter
    {
        public void Dispose()
        {

        }

        public void WriteLog(LogLevel level, string message, string name, Exception exception, EventId eventId)
        {
            File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"), $"{name}  {message}");
        }
    }
}
