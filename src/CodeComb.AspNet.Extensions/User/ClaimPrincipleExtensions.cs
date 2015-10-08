using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Security.Claims
{
    public static class ClaimPrincipleExtensions
    {
        public static bool AnyRoles(this ClaimsPrincipal self, string Roles)
        {
            var roles = Roles.Split(',');
            for (var i = 0; i < roles.Count(); i++)
                roles[i] = roles[i].Trim(' ');
            foreach (var r in roles)
                if (self.IsInRole(r))
                    return true;
            return false;
        }

        public static bool AnyRolesOrClaims(this ClaimsPrincipal self, string Roles, List<Claim> Claims)
        {
            var roles = Roles.Split(',');
            for (var i = 0; i < roles.Count(); i++)
                roles[i] = roles[i].Trim(' ');
            foreach (var r in roles)
                if (self.IsInRole(r))
                    return true;
            foreach(var c in Claims)
            {
                if (self.HasClaim(c.Type, c.Value))
                    return true;
            }
            return false;
        }

        public static bool AnyRolesOrClaims(this ClaimsPrincipal self, string Roles, string Types, string Value)
        {
            var tmp = Types.Split(',');
            var claims = new List<Claim>();
            foreach(var c in tmp)
            {
                claims.Add(new Claim(c.Trim(' '), Value));
            }
            return self.AnyRolesOrClaims(Roles, claims);
        }
    }
}
