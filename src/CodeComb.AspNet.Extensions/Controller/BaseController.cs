using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.PlatformAbstractions;

namespace Microsoft.AspNet.Mvc
{
    public abstract partial class BaseController : Controller
    {
        public string AppRoot { get { return Resolver?.GetService<IApplicationEnvironment>().ApplicationBasePath; } }

        public string WebRoot { get { return Resolver?.GetService<IHostingEnvironment>().WebRootPath; } }

        public IConfiguration Configuration { get { return Resolver?.GetService<IConfiguration>(); } }
        
        public CodeComb.AspNet.Extensions.SmartCookies.SmartCookies Cookies { get { return Resolver?.GetService<CodeComb.AspNet.Extensions.SmartCookies.SmartCookies>(); } }
        
        public CodeComb.AspNet.Extensions.Template.Template Template { get { return Resolver?.GetService<CodeComb.AspNet.Extensions.Template.Template>(); } }

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
