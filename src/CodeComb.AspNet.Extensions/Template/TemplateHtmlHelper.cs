using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.DependencyInjection;
using CodeComb.AspNet.Extensions.Template;

namespace Microsoft.AspNet.Mvc.Rendering
{
    public static class TemplateHtmlHelper
    {
        public static TemplateInfo GetTemplateInfo(this IHtmlHelper self)
        {
            var template = self.ViewContext.HttpContext.ApplicationServices.GetRequiredService<Template>();
            return template.Current;
        }

        public static string GetTemplateIdentifier(this IHtmlHelper self)
        {
            var template = self.ViewContext.HttpContext.ApplicationServices.GetRequiredService<Template>();
            return template.Current.Identifier;
        }
    }
}
