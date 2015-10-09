using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Entity;
using CodeComb.AspNet.Extensions.Sample.Models;

namespace CodeComb.AspNet.Extensions.Sample
{
    public class Startup
    {
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Add smart cookies
            services.AddSmartCookies();

            // Add entity framework and in-memory storage
            services.AddEntityFramework()
                .AddInMemoryDatabase()
                .AddDbContext<SampleContext>(x => x.UseInMemoryDatabase());

            // Add mvc with multi template engine and cookie based template provider
            services.AddMvc()
                .AddTemplate()
                .AddCookieTemplateProvider();

            services.AddUser<Models.User, string>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();

            app.UseAutoAjax();

            app.UseMvc(x => x.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"));
        }
    }
}
