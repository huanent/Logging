using Microsoft.Extensions.Logging;
using System;

namespace Huanent.Logging
{
    public interface ILoggerWriter
    {
        void WriteLog(LogLevel level, string message, string name, Exception exception, EventId eventId);
    }
}
