using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Http;

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
                Requires = "Roles",
                Hint = roles
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
