using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeComb.AspNet.Extensions.Cookies;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CookiesExtensions
    {
        public static IServiceCollection AddSmartCookies(this IServiceCollection self)
        {
            return self.AddScoped<Cookies>();
        }
    }
}
