using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Http;

namespace Microsoft.AspNet.Mvc
{
    public abstract partial class BaseController : Controller
    {
        [NonAction]
        [Obsolete]
        protected IActionResult Prompt(Prompt prompt)
        {
            Response.StatusCode = prompt.StatusCode;
            return View("Prompt", prompt);
        }

        [NonAction]
        protected IActionResult Prompt(Action<Prompt> setupPrompt)
        {
            var prompt = new Prompt();
            setupPrompt(prompt);
            Response.StatusCode = prompt.StatusCode;
            return View("Prompt", prompt);
        }
    }
}
