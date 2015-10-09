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
        public static IServiceCollection AddUser<TUser, TKey>(this IServiceCollection self)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            return self.AddScoped<User<TKey, TUser>>();
        }
    }
}
