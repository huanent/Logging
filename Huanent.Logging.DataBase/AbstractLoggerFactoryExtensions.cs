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
            builder.Services.AddScoped<ILoggerWriter, T>();
            return builder;
        }

        public static ILoggerFactory AddAbstract(this ILoggerFactory factory, IHttpContextAccessor httpContextAccessor, IServiceScopeFactory serviceScopeFactory)
        {
            return AddAbstract(factory, LogLevel.Information, httpContextAccessor, serviceScopeFactory);
        }

        public static ILoggerFactory AddAbstract(this ILoggerFactory factory, Func<string, LogLevel, bool> filter, IHttpContextAccessor httpContextAccessor, IServiceScopeFactory serviceScopeFactory)
        {
            factory.AddProvider(new AbstractLoggerProvider(filter, httpContextAccessor, serviceScopeFactory));
            return factory;
        }

        public static ILoggerFactory AddAbstract(this ILoggerFactory factory, LogLevel minLevel, IHttpContextAccessor httpContextAccessor, IServiceScopeFactory serviceScopeFactory)
        {
            return AddAbstract(factory, (_, logLevel) => logLevel >= minLevel, httpContextAccessor, serviceScopeFactory);
        }
    }
}