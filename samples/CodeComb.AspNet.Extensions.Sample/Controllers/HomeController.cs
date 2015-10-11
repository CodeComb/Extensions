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
        public IActionResult Index()
        {
            ViewBag.TemplatesCount = Template.Collection.Count;
            ViewBag.CurrentTemplate = Template.Current.Title;
            ViewBag.UserCount = DB.Users.Count();
            return View();
        }

        public IActionResult SetTemplate(string identifier, [FromHeader]string Referer)
        {
            Cookies["ASPNET_TEMPLATE"] = "default";
            return Redirect(Referer ?? "/");
        }

        [AnyRoles("Root, Master")]
        public IActionResult Test()
        {
            return Content("You are able to access this action.");
        }
    }
}
