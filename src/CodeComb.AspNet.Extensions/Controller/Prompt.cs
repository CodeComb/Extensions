using System;
using System.Linq;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.Http;

namespace Microsoft.AspNet.Mvc
{
    public abstract partial class BaseController : Controller
    {
        [NonAction]
        protected IActionResult Prompt(Prompt prompt)
        {
            Response.StatusCode = prompt.StatusCode;
            return View("Prompt", prompt);
        }
    }
}
