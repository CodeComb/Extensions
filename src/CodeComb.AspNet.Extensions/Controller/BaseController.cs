using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNet.Mvc.Filters;

namespace Microsoft.AspNet.Mvc
{
    public abstract partial class BaseController : Controller
    {
        [FromServices]
        public IConfiguration Configuration { get; set; }

        [FromServices]
        public CodeComb.AspNet.Extensions.Cookies.Cookies Cookies { get; set; }

        [FromServices]
        public CodeComb.AspNet.Extensions.Template.Template Template { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Prepare();
            base.OnActionExecuting(context);
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Prepare();
            return base.OnActionExecutionAsync(context, next);
        }

        public virtual void Prepare()
        {
        }
    }
}
