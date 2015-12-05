using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Entity;
using CodeComb.AspNet.Extensions.Sample.Models;

namespace CodeComb.AspNet.Extensions.Sample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add smart cookies
            services.AddSmartCookies();

            // Add entity framework and in-memory storage
            services.AddEntityFramework()
                .AddInMemoryDatabase()
                .AddDbContext<SampleContext>(x => x.UseInMemoryDatabase());

            // Add identity
            services.AddIdentity<Models.User, IdentityRole>(x =>
            {
                x.Password.RequiredLength = 0;
                x.Password.RequireDigit = false;
                x.Password.RequireLowercase = false;
                x.Password.RequireNonLetterOrDigit = false;
                x.Password.RequireUppercase = false;
                x.User.RequireUniqueEmail = false;
                x.User.AllowedUserNameCharacters = null;
            })
                .AddEntityFrameworkStores<SampleContext>()
                .AddDefaultTokenProviders();

            // Add mvc with multi template engine and cookie based template provider
            services.AddMvc()
                .AddTemplate()
                .AddCookieTemplateProvider();

            services.AddSmartUser<User, string>();
        }

        public async void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(minLevel: LogLevel.Warning);
            loggerFactory.AddDebug();

            app.UseStaticFiles();
            app.UseAutoAjax();
            app.UseMvc(x => x.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"));
            app.UseAutoAjax();

            await SampleData.InitDB(app.ApplicationServices);
        }
    }
}
