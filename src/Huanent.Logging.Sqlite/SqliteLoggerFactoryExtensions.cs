using Huanent.Logging.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.Extensions.Logging
{
    public static class SqliteLoggerFactoryExtensions
    {
        public static ILoggerFactory AddSqlite(this ILoggerFactory factory, LogLevel minLevel)
        {
            return AddSqlite(
               factory,
               o => o.Filter = (_, logLevel) => logLevel >= minLevel);
        }

        public static ILoggerFactory AddSqlite(this ILoggerFactory factory, Action<SqliteLoggerOptions> options = null)
        {
            var option = new SqliteLoggerOptions();
            options?.Invoke(option);
            factory.AddProvider(new SqliteLoggerProvider(option));
            return factory;
        }

        public static ILoggerFactory AddSqlite(this ILoggerFactory factory)
        {
            return AddSqlite(factory, LogLevel.Information);
        }

        public static ILoggingBuilder AddSqlite(this ILoggingBuilder builder, Action<SqliteLoggerOptions> options = null)
        {
            var option = new SqliteLoggerOptions();
            options?.Invoke(option);
            builder.Services.AddSingleton(option);
            builder.Services.AddSingleton<ILoggerProvider, SqliteLoggerProvider>();
            return builder;
        }
    }
}
