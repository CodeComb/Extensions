using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CodeComb.AspNet.Extensions.Sample.Models
{
    public static class SampleData
    {
        public async static Task InitDB(IServiceProvider services)
        {
            using (var DB = services.GetRequiredService<SampleContext>())
            {
                if (DB.Database.EnsureCreated())
                {
                    var UserManager = services.GetRequiredService<UserManager<User>>();
                    var RoleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    var role = new IdentityRole { Name = "Root" };
                    await RoleManager.CreateAsync(role);

                    var user = new User { UserName = "root" };
                    await UserManager.CreateAsync(user, "123456");
                    await UserManager.AddToRoleAsync(user, "Root");
                }
            }
        }
    }
}
