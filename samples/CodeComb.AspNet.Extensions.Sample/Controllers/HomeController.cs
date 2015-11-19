using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.DependencyInjection;
using CodeComb.AspNet.Extensions.Template;
using CodeComb.AspNet.Extensions.Sample.Models;

namespace CodeComb.AspNet.Extensions.Sample.Controllers
{
    public class HomeController : BaseController
    {
        [Inject]
        public new SmartCookies.SmartCookies  Cookies { get; set; }

        public IActionResult Index()
        {
            ViewBag.TemplatesCount = Template.Collection.Count;
            ViewBag.CurrentTemplate = Template.Current.Title;
            ViewBag.UserCount = DB.Users.Count();
            ViewBag.TestInject = Cookies["ASPNET_TEMPLATE"];
            return View();
        }

        public IActionResult SetTemplate(string identifier, [FromHeader]string Referer)
        {
            Cookies["ASPNET_TEMPLATE"] = identifier;
            return Redirect(Referer ?? "/");
        }

        [AnyRoles("Root, Master")]
        public IActionResult Test()
        {
            return Content("You are able to access this action.");
        }

        public IActionResult Ajax()
        {
            var ret = new List<string>
            {
                "abc",
                "123",
                "!@#"
            };
            return AjaxPagedView(ret, ".lst-ajax-test", 2, AjaxPerformanceType.Tradition);
        }
    }
}
