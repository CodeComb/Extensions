﻿using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Http;

namespace Microsoft.AspNet.Mvc
{
    public class AnyRolesOrClaimsAttribute : ActionFilterAttribute
    {
        private string[] claimTypes;
        private string claimValue;
        private string[] roles;
        private string routeField;

        public AnyRolesOrClaimsAttribute(string Roles, string Types, string RouteField = "id")
        {
            claimTypes = Types.Split(',');
            for (var i = 0; i < claimTypes.Count(); i++)
                claimTypes[i] = claimTypes[i].Trim(' ');
            routeField = RouteField;
            roles = Roles.Split(',');
            for (var i = 0; i < roles.Count(); i++)
                roles[i] = roles[i].Trim(' ');
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.RouteData.Values[routeField] == null)
                claimValue = null;
            else
                claimValue = context.RouteData.Values[routeField].ToString();

            var user = context.HttpContext.User;
            foreach (var r in roles)
            {
                if (user.IsInRole(r))
                {
                    base.OnActionExecuting(context);
                    return;
                }
            }
            if (!string.IsNullOrEmpty(claimValue))
            {
                foreach(var c in claimTypes)
                {
                    if (user.HasClaim(c, claimValue))
                    {
                        base.OnActionExecuting(context);
                        return;
                    }
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
                Requires = "ClaimsOrRoles",
                Hint = new { Roles = roles, Claims = claimTypes }
            };
            var services = context.HttpContext.RequestServices;
            context.Result = new ViewResult
            {
                StatusCode = prompt.StatusCode,
                TempData = new TempDataDictionary(services.GetRequiredService<IHttpContextAccessor>().HttpContext, services.GetRequiredService<ITempDataProvider>()),
                ViewName = "Prompt",
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState) { Model = prompt }
            };
        }
    }
}
