using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using CodeComb.AspNet.Extensions.User;

namespace Microsoft.AspNet.Mvc
{
    public abstract class BaseController<TContext, TUser, TKey> : BaseController<TContext>
    where TKey : IEquatable<TKey>
    where TUser : IdentityUser<TKey>
    {
        [FromServices]
        public UserManager<TUser> UserManager { get; set; }

        [FromServices]
        public SignInManager<TUser> SignInManager { get; set; }

        [FromServices]
        public RoleManager<TUser> RoleManager { get; set; }

        [FromServices]
        public new User<TUser, TKey> User { get; set; }
    }
}
