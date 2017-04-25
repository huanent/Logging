using Huanent.Logging.File;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Huanent.Logging
{
    public static class UseLoggingExtensions
    {
        /// <summary>
        /// 开启日志
        /// </summary>
        /// <param name="app"></param>
        /// <param name="option"></param>
        public static void UseLogging(this IApplicationBuilder app, IOptions<LoggingOptions> option = null)
        {
            var loggerFactory = app.ApplicationServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            var env = app.ApplicationServices.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;
            option = option ?? app.ApplicationServices.GetService(typeof(IOptions<LoggingOptions>)) as IOptions<LoggingOptions>;
            if (option == null) throw new ArgumentException(nameof(LoggingOptions));

            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(option.Value.ConsoleLogLevel);
                loggerFactory.AddDebug(option.Value.DebugLogLevel);
            }

            loggerFactory.AddFileLog(new FileWriterLoggerOptions
            {
                BasePath = $"{env.ContentRootPath}\\log",
                LogLevel = option.Value.FileLogLevel
            });
        }
    }
}
