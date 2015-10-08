using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Internal;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Mvc.ViewFeatures;

namespace Microsoft.AspNet.Mvc
{
    public class AnyClaimsAttribute : ActionFilterAttribute
    {
        private string[] claimTypes;
        private string claimValue;
        private string routeField;

        public AnyClaimsAttribute(string Types, string RouteField = "id")
        {
            claimTypes = Types.Split(',');
            for (var i = 0; i < claimTypes.Count(); i++)
                claimTypes[i] = claimTypes[i].Trim(' ');
            routeField = RouteField;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.RouteData.Values[routeField] == null)
            {
                HandleUnauthorizedRequest(context);
                return;
            }
            else
            {
                claimValue = context.RouteData.Values[routeField].ToString();
            }

            var user = context.HttpContext.User;
            foreach(var c in claimTypes)
            {
                if (user.HasClaim(c, claimValue))
                {
                    base.OnActionExecuting(context);
                    return;
                }
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
                Requires = "Claims",
                Hint = claimTypes
            };

            context.Result = new ViewResult { StatusCode = 403, ViewData = new ViewDataDictionary<Prompt>(null, prompt), ViewName = "Prompt" };
        }
    }
}
