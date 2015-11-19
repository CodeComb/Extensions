using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace Microsoft.AspNet.Mvc
{
    public abstract partial class BaseController : Controller
    {
        [NonAction]
        protected IActionResult PagedView<TView, TModel>(
           IEnumerable<TModel> Source,
           int PageSize = 50,
           string ViewPath = null)
           where TView : class
           where TModel : IConvertible<TView>
        {
            int? p;
            try
            {
                if (Request.Query["p"].Count > 0)
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
            var ret = new List<TView>();
            foreach (var item in Source)
            {
                var tmp = (item as IConvertible<TView>).ToType();
                ret.Add(tmp);
            }
            if (string.IsNullOrEmpty(ViewPath))
                return View(ret);
            else
                return View(ViewPath, ret);
        }
    }
}
