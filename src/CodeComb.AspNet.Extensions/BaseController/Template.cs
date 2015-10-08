using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Microsoft.AspNet.Mvc
{
    public abstract partial class BaseController : Controller
    {
        [NonAction]
        [Obsolete]
        public IActionResult TemplatedView()
        {
            return TemplatedView(null, null);
        }

        [NonAction]
        [Obsolete]
        public IActionResult TemplatedView(object model)
        {
            return TemplatedView(null, model);
        }

        [NonAction]
        [Obsolete]
        public IActionResult TemplateView(string viewName)
        {
            return TemplatedView(viewName, null);
        }

        [NonAction]
        [Obsolete]
        public IActionResult TemplatedView(string viewName, object model)
        {
            var template = Cookies["_template"];
            viewName = viewName == null
                ? ("~/Views/" + CurrentTemplate.Folder + "/" + ActionContext.RouteData.Values["controller"] + "/" + ActionContext.ActionDescriptor.Name)
                : viewName.Replace("{template}", template ?? Template.DefaultTemplate);
            return View(viewName, model);
        }
    }
}
