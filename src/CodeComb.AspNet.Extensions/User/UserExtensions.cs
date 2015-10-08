using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using CodeComb.AspNet.Extensions.User;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UserExtensions
    {
        public static IServiceCollection AddUser<TKey, TUser>(this IServiceCollection self)
            where TKey : IEquatable<TKey>
            where TUser : IdentityUser<TKey>
        {
            return self.AddScoped<User<TKey, TUser>>();
        }
    }
}
