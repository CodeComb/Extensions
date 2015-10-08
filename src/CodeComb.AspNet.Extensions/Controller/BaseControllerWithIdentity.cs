using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Microsoft.AspNet.Mvc
{
    public abstract class BaseController<TContext, TKey, TUser> : BaseController<TContext>
    where TKey : IEquatable<TKey>
    where TUser : IdentityUser<TKey>
    {
        public UserManager<TUser> UserManager { get; set; }

        public SignInManager<TUser> SignInManager { get; set; }

        public RoleManager<TUser> RoleManager { get; set; }

        public TUser CurrentUser { get; set; }
    }
}
