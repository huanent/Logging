using Huanent.Logging;
using Huanent.Logging.File;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.Logging
{
    public static class FileLoggerFactoryExtensions
    {
        public static ILoggingBuilder AddFile(this ILoggingBuilder builder, Action<FileLogOptions> options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            builder.Services.Configure(options);
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerWriter, FileLoggerWriter>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, FileLoggerProvider>());
            return builder;
        }
    }
}
