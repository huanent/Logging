using Huanent.Logging.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
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
        public MyLogWriter(IHttpContextAccessor httpContextAccessor, IServiceScopeFactory serviceScopeFactory)
        {
        }

        public void WriteLog(LogLevel level, string message, string name, Exception exception, EventId eventId)
        {
        }
    }
}
