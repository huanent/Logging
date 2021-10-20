using Microsoft.Extensions.Logging;
using System;

namespace Huanent.Logging.Core
{
    public interface ILogWriter
    {
        void WriteLog(LogLevel level, string message, string name, Exception exception, EventId eventId);
    }
}
