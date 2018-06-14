using Huanent.Logging.File.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AddLoggingFileUIExtensions
    {
        public static void AddLoggingFileUI(this IServiceCollection services)
        {
            services.ConfigureOptions(typeof(LoggingFileUIConfigureOptions));
        }
    }
}
