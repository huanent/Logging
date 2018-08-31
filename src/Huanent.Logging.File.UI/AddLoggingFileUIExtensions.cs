using Huanent.Logging.File.UI;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AddLoggingFileUIExtensions
    {
        public static void AddLoggingFileUI(
            this IServiceCollection services,
            Action<LoggingFileUIOptions> options = null)
        {
            var option = new LoggingFileUIOptions();
            options?.Invoke(option);
            services.AddSingleton(option);
            services.AddSingleton<AuthService>();
            services.AddSingleton<ConfigService>();
            services.ConfigureOptions(typeof(LoggingFileUIConfigureOptions));
        }
    }
}