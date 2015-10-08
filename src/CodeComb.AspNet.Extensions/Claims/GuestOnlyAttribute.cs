using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace Microsoft.AspNet.Mvc
{
    public class GuestOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
                HandleUnauthorizedRequest(context);
            else
                base.OnActionExecuting(context);
        }

        public virtual void HandleUnauthorizedRequest(ActionExecutingContext context)
        {
            var prompt = new Prompt
            {
                Title = "Sign Out First",
                StatusCode = 403,
                Details = "You must be a guest to visit this page."
            };

            context.Result = new ViewResult { StatusCode = 403, ViewData = new ViewDataDictionary<Prompt>(null, prompt), ViewName = "Prompt" };
        }
    }
}
