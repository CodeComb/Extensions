using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;

namespace CodeComb.AspNet.Extensions.Template
{
    public class CookieRequestTemplateProvider : IRequestTemplateProvider
    {
        public HttpContext HttpContext { get; }

        public string CookieField { get; }

        public CookieRequestTemplateProvider(IHttpContextAccessor accessor, string cookieField = "ASPNET_TEMPLATE")
        {
            CookieField = cookieField;
            HttpContext = accessor.HttpContext;
        }

        public string DetermineRequestTemplate()
        {
            try
            {
                return HttpContext.Request.Cookies[CookieField].ToString();
            }
            catch
            {
                return null;
            }
        }
    }
}
