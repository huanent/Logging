using Huanent.Logging.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstract;
using System;

namespace Microsoft.Extensions.Logging
{

    public static class AbstractLoggerFactoryExtensions
    {
        public static ILoggingBuilder AddAbstract<T>(this ILoggingBuilder builder) where T : class, ILoggerWriter
        {
            builder.Services.AddSingleton<ILoggerProvider, AbstractLoggerProvider>();
            builder.Services.AddSingleton<ILoggerWriter, T>();
            return builder;
        }

        public static ILoggerFactory AddAbstract(this ILoggerFactory factory, ILoggerWriter loggerWriter)
        {
            return AddAbstract(factory, LogLevel.Information, loggerWriter);
        }

        public static ILoggerFactory AddAbstract(this ILoggerFactory factory, Func<string, LogLevel, bool> filter, ILoggerWriter loggerWriter)
        {
            factory.AddProvider(new AbstractLoggerProvider(filter, loggerWriter));
            return factory;
        }

        public static ILoggerFactory AddAbstract(this ILoggerFactory factory, LogLevel minLevel, ILoggerWriter loggerWriter)
        {
            return AddAbstract(factory, (_, logLevel) => logLevel >= minLevel, loggerWriter);
        }
    }
}