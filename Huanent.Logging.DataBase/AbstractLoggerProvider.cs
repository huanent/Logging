using Huanent.Logging.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.Extensions.Logging.Abstract
{
    [ProviderAlias("Abstract")]
    public class AbstractLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;
        IHttpContextAccessor _httpContextAccessor;
        IServiceScopeFactory _serviceScopeFactory;

        public AbstractLoggerProvider(IHttpContextAccessor httpContextAccessor, IServiceScopeFactory serviceScopeFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public AbstractLoggerProvider(Func<string, LogLevel, bool> filter, IHttpContextAccessor httpContextAccessor, IServiceScopeFactory serviceScopeFactory)
            : this(httpContextAccessor, serviceScopeFactory)
        {
            _filter = filter;
            _httpContextAccessor = httpContextAccessor;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public ILogger CreateLogger(string name)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            ILoggerWriter loggerWriter;
            if (httpContext != null) loggerWriter = httpContext.RequestServices.GetService<ILoggerWriter>();
            else loggerWriter = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<ILoggerWriter>();
            return new AbstractLogger(name, _filter, loggerWriter);
        }

        public void Dispose()
        {
        }
    }
}
