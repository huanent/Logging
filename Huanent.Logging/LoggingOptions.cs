using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Huanent.Logging
{
    public class LoggingOptions : IOptions<LoggingOptions>
    {
        public LogLevel ConsoleLogLevel { get; set; } = LogLevel.Information;

        public LogLevel DebugLogLevel { get; set; } = LogLevel.Trace;

        public LogLevel FileLogLevel { get; set; } = LogLevel.Warning;

        public LoggingOptions Value => this;
    }
}
