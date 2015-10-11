using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Http;

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
            var services = context.HttpContext.ApplicationServices;
            context.Result = new ViewResult
            {
                StatusCode = prompt.StatusCode,
                TempData = new TempDataDictionary(services.GetRequiredService<IHttpContextAccessor>(), services.GetRequiredService<ITempDataProvider>()),
                ViewName = "Prompt",
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState) { Model = prompt }
            };
        }
    }
}
