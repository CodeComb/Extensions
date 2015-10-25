using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace CodeComb.AspNet.Extensions.Sample.Controllers
{
    public class AccountController : BaseController
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, bool remember)
        {
            var result = await SignInManager.PasswordSignInAsync(username, password, remember, false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                return Prompt(x => 
                {
                    x.Title = "Login failed";
                    x.Details = "Please check your username or password.";
                    x.StatusCode = 403;
                    x.RedirectUrl = Url.Link("default", new { controller = "Login", action = "Account" });
                    x.RedirectText = "Retry";
                });
        }
    }
}
