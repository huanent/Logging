using Huanent.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.Logging
{
    public static class LoggerFactoryExtensions
    {
        public static ILoggingBuilder AddImplementation<T>(this ILoggingBuilder builder) where T : class, ILoggerWriter
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerWriter, T>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, LoggerProvider>());
            return builder;
        }
    }
}
