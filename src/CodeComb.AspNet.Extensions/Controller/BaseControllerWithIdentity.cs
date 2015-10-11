using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using CodeComb.AspNet.Extensions.User;

namespace Microsoft.AspNet.Mvc
{
    public abstract class BaseController<TContext, TUser, TKey> : BaseController<TContext>
    where TKey : IEquatable<TKey>
    where TUser : IdentityUser<TKey>
    {
        public UserManager<TUser> UserManager { get { return Resolver?.GetService<UserManager<TUser>>(); } }
        
        public SignInManager<TUser> SignInManager { get { return Resolver?.GetService<SignInManager<TUser>>(); } }
        
        public RoleManager<TUser> RoleManager { get { return Resolver?.GetService<RoleManager<TUser>>(); } }
        
        public new User<TUser, TKey> User { get { return Resolver?.GetService<User<TUser, TKey>>(); } }
    }
}
