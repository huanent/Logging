using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Huanent.Logging.File.Sample
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseStaticFiles();
            var logger = loggerFactory.CreateLogger<Startup>();
            logger.LogWarning("写入警告");
            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLoggingFileUI();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
    }
}