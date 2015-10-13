using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Razor;
using CodeComb.AspNet.Extensions.Template;

namespace Microsoft.Framework.DependencyInjection
{
    public static class TemplateExtension
    {
        public static IMvcBuilder AddTemplate(this IMvcBuilder self)
        {
            self.Services
                .AddSingleton<TemplateCollection>()
                .AddSingleton<IRazorViewEngine, TemplateEngine>()
                .AddScoped<Template>();

            return self.AddViewOptions(x =>
            {
                foreach (var v in x.ViewEngines)
                    if (!(v is TemplateEngine))
                        x.ViewEngines.Remove(v);
            });
        }

        public static IMvcBuilder AddQueryStringTemplateProvider(this IMvcBuilder self, string QueryField = "template")
        {
            self.Services.AddScoped<IRequestTemplateProvider>(x => new QueryStringRequestTemplateProvider(x.GetRequiredService<IHttpContextAccessor>(), QueryField));
            return self;
        }

        public static IMvcBuilder AddCookieTemplateProvider(this IMvcBuilder self, string CookieField = "ASPNET_TEMPLATE")
        {
            self.Services.AddScoped<IRequestTemplateProvider>(x => new CookieRequestTemplateProvider(x.GetRequiredService<IHttpContextAccessor>(), CookieField));
            return self;
        }
    }
}
