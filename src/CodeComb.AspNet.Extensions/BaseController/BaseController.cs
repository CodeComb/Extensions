using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Filters;
using Newtonsoft.Json;
using Microsoft.AspNet.Mvc.ViewFeatures;
using CodeComb.Localization.Models;

namespace Microsoft.AspNet.Mvc
{
    public abstract class BaseController<TUser, TContext> : BaseController<TUser, TContext, long> where TUser : IdentityUser<long>
    {
    }

    public abstract class StringIdBaseController<TUser, TContext> : BaseController<TUser, TContext, string> where TUser : IdentityUser<string>
    {
    }

    public abstract class BaseController<TUser, TContext, TKey> : BaseController<TUser, TContext, TKey, UserManager<TUser>, RoleManager<TUser>, SignInManager<TUser>> where TUser : IdentityUser<TKey> where TKey : IEquatable<TKey>
    {
    }

    public abstract class BaseController<TUser, TContext, TKey, TUserManager, TRoleManager, TSignInManager> : _BaseController<TUser, TContext, TKey, TUserManager, TRoleManager, TSignInManager>
    where TKey : IEquatable<TKey>
    where TUser : IdentityUser<TKey>
    where TUserManager : UserManager<TUser>
    where TRoleManager : RoleManager<TUser>
    where TSignInManager : SignInManager<TUser>
    {
        public CodeCombLocalization LocalizationManager { get; set; }
        public override void Prepare()
        {
            LocalizationManager = Resolver.GetService<CodeCombLocalization>();
            base.Prepare();
        }
    }

    public abstract partial class BaseController : Controller
    {
        public IConfiguration Configuration { get; set; }

        public CodeComb.vNext.Models.Cookies Cookies { get; set; }

        public CodeComb.vNext.Models.Template CurrentTemplate { get; private set; }

        public Template Template { get; set; }

        public List<CodeComb.vNext.Models.Template> Templates { get; private set; } = null;
        
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
            Configuration = Resolver.GetService<IConfiguration>();
            ViewBag.Configuration = Configuration;
            var httpContextAccessor = Resolver.GetService<IHttpContextAccessor>();
            Cookies = new CodeComb.vNext.Models.Cookies(httpContextAccessor.HttpContext);
            var template = Resolver.GetService<Template>();
            this.Template = template;
            if (Template != null)
            {
                Templates = template.Templates;
                if (Cookies["_template"] != null && template.Templates.Any(x => x.Folder == Cookies["_template"]))
                    CurrentTemplate = template.Templates.Where(x => x.Folder == Cookies["_template"]).First();
                else if (template.Templates.Any(x => x.Folder == template.DefaultTemplate))
                    CurrentTemplate = template.Templates.Where(x => x.Folder == template.DefaultTemplate).First();
                else
                    CurrentTemplate = null;
                ViewData["_CurrentTemplate"] = CurrentTemplate;
            }
        }
    }

    public abstract class BaseController<TContext> : BaseController
    {
        public virtual TContext DB { get; set; }

        public override void Prepare()
        {
            base.Prepare();
            DB = Resolver.GetService<TContext>();
        }
    }

    public abstract partial class _BaseController<TUser, TContext, TKey, TUserManager, TRoleManager, TSignInManager> : BaseController<TContext>
        where TKey : IEquatable<TKey> 
        where TUser : IdentityUser<TKey> 
        where TUserManager: UserManager<TUser> 
        where TRoleManager:RoleManager<TUser> 
        where TSignInManager: SignInManager<TUser>
    {
        public UserManager<TUser> UserManager { get; set; }

        public SignInManager<TUser> SignInManager { get; set; }

        public RoleManager<TUser> RoleManager { get; set; }

        public TUser CurrentUser { get; set; }
        
        public override void Prepare()
        {
            base.Prepare();

            UserManager = Resolver.GetService<TUserManager>();
            RoleManager = Resolver.GetService<TRoleManager>();
            SignInManager = Resolver.GetService<TSignInManager>();
            if (User.IsSignedIn())
            {
                var Type = typeof(TUser);
                var tmp = User.GetUserName();
                try
                {
                    CurrentUser = UserManager.Users.Where(x => x.UserName.Equals(tmp)).Single();
                }
                catch
                {
                    SignInManager.SignOutAsync().Wait();
                }
            }
            else
            {
                CurrentUser = null;
            }
        }

    }

    public abstract class LocalizationBaseController<TUser, TContext> : LocalizationBaseController<TUser, TContext, long>
        where TUser : IdentityUser<long>
        where TContext : ILocalizationContext
    {
    }

    public abstract class StringIdLocalizationBaseController<TUser, TContext> : LocalizationBaseController<TUser, TContext, string>
        where TUser : IdentityUser<string>
        where TContext : ILocalizationContext
    {
    }

    public abstract class LocalizationBaseController<TUser, TContext, TKey>: LocalizationBaseController<TUser, TContext, TKey, UserManager<TUser>, RoleManager<TUser>, SignInManager<TUser>> 
        where TUser : IdentityUser<TKey> 
        where TKey : IEquatable<TKey>
        where TContext : ILocalizationContext
    {
    }

    public abstract class LocalizationBaseController<TContext> : BaseController<TContext>
        where TContext : ILocalizationContext
    {
        public CodeCombLocalization<TContext> LocalizationManager { get; set; }
        public override void Prepare()
        {
            base.Prepare();
            LocalizationManager = Resolver.GetService<CodeCombLocalization<TContext>>();
        }
    }

    public abstract class LocalizationBaseController<TUser, TContext, TKey, TUserManager, TRoleManager, TSignInManager> : _BaseController<TUser, TContext, TKey, TUserManager, TRoleManager, TSignInManager>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
        where TUserManager : UserManager<TUser>
        where TRoleManager : RoleManager<TUser>
        where TSignInManager : SignInManager<TUser>
        where TContext : ILocalizationContext
    {
        public CodeCombLocalization<TContext> LocalizationManager { get; set; }
        public override void Prepare()
        {
            base.Prepare();
            LocalizationManager = Resolver.GetService<CodeCombLocalization<TContext>>();
        }
    }
}
