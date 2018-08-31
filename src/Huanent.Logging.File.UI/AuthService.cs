using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Huanent.Logging.File.UI
{
    public class AuthService
    {
        private readonly ConfigService _configService;

        public AuthService(ConfigService configService)
        {
            _configService = configService;
        }

        public bool Check(HttpContext httpContext)
        {
            bool haveCookie = httpContext.Request.Cookies.TryGetValue(Constants.CookieName, out string enDate);
            if (!haveCookie) return false;

            var config = _configService.Get();
            if (config == null) return false;

            string dateStr = AES256Helper.Decrypt(enDate, config.AESKey);
            if (!DateTime.TryParse(dateStr, out var date)) return false;
            if (DateTime.Now > date) return false;

            string token = AES256Helper.Encrypt(DateTime.Now.AddMinutes(30).ToString(), config.AESKey);
            httpContext.Response.Cookies.Delete(Constants.CookieName);
            httpContext.Response.Cookies.Append(Constants.CookieName, token);
            return true;
        }
    }
}