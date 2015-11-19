using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;

namespace CodeComb.AspNet.Extensions.SmartUser
{
    public class SmartUser<TUser, TKey> : ClaimsPrincipal
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
    {
        public HttpContext HttpContext { get; }

        public UserManager<TUser> Manager { get; }

        private TUser _current = null;

        public bool IsSignedIn()
        {
            return HttpContext.User.IsSignedIn();
        }

        public new TUser Current
        {
            get
            {
                if (_current == null)
                {
                    if (!HttpContext.User.IsSignedIn()) return null;
                    var um = HttpContext.RequestServices.GetRequiredService<UserManager<TUser>>();
                    var Type = typeof(TUser);
                    object tmp = HttpContext.User.GetUserId();
                    TKey uid = (TKey)Convert.ChangeType(tmp, typeof(TKey));
                    try
                    {
                        _current = um.Users.Where(x => x.Id.Equals(uid)).Single();
                        return _current;
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                {
                    return _current;
                }
            }
        }

        public SmartUser(IHttpContextAccessor accessor, UserManager<TUser> userManager)
        {
            HttpContext = accessor.HttpContext;
            Manager = userManager;
            this.AddIdentity(new ClaimsIdentity(HttpContext.User.Identity));
            this.AddIdentities(HttpContext.User.Identities);
        }
    }
}
