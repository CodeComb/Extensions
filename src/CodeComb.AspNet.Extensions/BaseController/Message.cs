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
        public IActionResult Message(string Title, string Content, string ViewName = "Message")
        {
            ViewBag.Title = Title;
            ViewBag.Content = Content;
            return View(ViewName);
        }


        [NonAction]
        [Obsolete]
        public IActionResult TemplatedMessage(string Title, string Content, string ViewPath = "~/Views/{template}/Home/Message")
        {
            ViewBag.Title = Title;
            ViewBag.Content = Content;
            ViewPath = ViewPath.Replace("{template}", Cookies["_template"] ?? Template.DefaultTemplate);
            return View(ViewPath);
        }
    }
}
