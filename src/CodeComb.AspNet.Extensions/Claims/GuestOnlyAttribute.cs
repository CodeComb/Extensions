using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Http;

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
