using Huanent.Logging.File;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.File;
using System;

namespace Microsoft.Extensions.Logging
{
    public static class FileLoggerFactoryExtensions
    {
        public static ILoggingBuilder AddFile(this ILoggingBuilder builder, Action<FileLoggerOptions> options = null)
        {
            var option = new FileLoggerOptions();
            options?.Invoke(option);
            builder.Services.AddSingleton(option);
            builder.Services.AddSingleton<FileLoggerWriter>();
            builder.Services.AddSingleton<ILoggerProvider, FileLoggerProvider>();
            return builder;
        }

        public static ILoggerFactory AddFile(this ILoggerFactory factory)
        {
            return AddFile(factory, LogLevel.Information);
        }

        public static ILoggerFactory AddFile(this ILoggerFactory factory, Action<FileLoggerOptions> options = null)
        {
            var option = new FileLoggerOptions();
            options?.Invoke(option);
            factory.AddProvider(new FileLoggerProvider(option));
            return factory;
        }

        public static ILoggerFactory AddFile(this ILoggerFactory factory, LogLevel minLevel)
        {
            return AddFile(
               factory,
               o => o.Filter = (_, logLevel) => logLevel >= minLevel);
        }
    }
}