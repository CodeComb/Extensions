using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace Microsoft.AspNet.Mvc
{
    public abstract partial class BaseController : Controller
    {
        [NonAction]
        protected IActionResult PagedView<TModel>(
            IEnumerable<TModel> Source,
            int PageSize = 50,
            string ViewPath = null)
        {
            int? p;
            try
            {
                if (Request.Query["p"] != null)
                {
                    p = int.Parse(Request.Query["p"].ToString());
                }
                else if (RouteData.Values["p"] != null)
                {
                    p = int.Parse(RouteData.Values["p"].ToString());
                }
                else
                {
                    p = 1;
                }
            }
            catch
            {
                p = 1;
            }
            ViewData["PagerInfo"] = Pager.Divide(ref Source, PageSize, p.Value);
            if (string.IsNullOrEmpty(ViewPath))
                return View(Source);
            else
                return View(ViewPath, Source);
        }

        [NonAction]
        [Obsolete]
        public IActionResult PagedTemplatedView<TModel>(IEnumerable<TModel> Source,
            int PageSize = 50,
            string viewName = null)
        {
            int? p;
            try
            {
                if (Request.Query["p"] != null)
                {
                    p = int.Parse(Request.Query["p"].ToString());
                }
                else if (RouteData.Values["p"] != null)
                {
                    p = int.Parse(RouteData.Values["p"].ToString());
                }
                else
                {
                    p = 1;
                }
            }
            catch
            {
                p = 1;
            }
            ViewData["PagerInfo"] = Pager.Divide(ref Source, PageSize, p.Value);
            var template = Cookies["_template"];
            viewName = viewName == null
                ? ("~/Views/" + template ?? Template.DefaultTemplate + "/" + ActionContext.RouteData.Values["controller"] + "/" + ActionContext.ActionDescriptor.Name)
                : viewName.Replace("{template}", template ?? Template.DefaultTemplate);
            if (string.IsNullOrEmpty(viewName))
            {
                return View(Source);
            }
            else
            {
                return View(viewName, Source);
            }
        }
    }
}
