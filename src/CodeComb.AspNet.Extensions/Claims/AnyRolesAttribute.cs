using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Mvc.ViewFeatures;

namespace Microsoft.AspNet.Mvc
{
    public class AnyRolesAttribute : ActionFilterAttribute
    {
        private string[] roles;

        public AnyRolesAttribute(string Roles)
        {
            roles = Roles.Split(',');
            for (var i = 0; i < roles.Count(); i++)
                roles[i] = roles[i].Trim(' ');
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var r in roles)
            {
                if (context.HttpContext.User.IsInRole(r))
                    base.OnActionExecuting(context);
            }
            HandleUnauthorizedRequest(context);
        }

        public virtual void HandleUnauthorizedRequest(ActionExecutingContext context)
        {
            var prompt = new Prompt
            {
                Title = "Permission Denied",
                StatusCode = 403,
                Details = "You must sign in with a higher power account.",
                Requires = "Roles",
                Hint = roles
            };

            context.Result = new ViewResult { StatusCode = 403, ViewData = new ViewDataDictionary<Prompt>(null, prompt), ViewName = "Prompt" };
        }
    }
}
