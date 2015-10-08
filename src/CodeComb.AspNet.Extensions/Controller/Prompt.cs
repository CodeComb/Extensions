using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Http;

namespace Microsoft.AspNet.Mvc
{
    public abstract partial class BaseController : Controller
    {
        [NonAction]
        protected IActionResult Prompt(Prompt prompt)
        {
            return View("Prompt", prompt);
        }
    }
}
