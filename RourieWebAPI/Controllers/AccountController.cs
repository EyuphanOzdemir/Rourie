using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RourieWebAPI.Models;
using DBAccessLibrary;
using Microsoft.Extensions.Options;

namespace RourieWebAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private CookieAuthenticationOptions _cookieAuthenticationOptions;
        private readonly string loginPath;

        public AccountController(DataContext context, IOptionsMonitor<CookieAuthenticationOptions> options)
        {
            _context = context;
            this._cookieAuthenticationOptions = options.CurrentValue;
            loginPath = options.Get("").LoginPath;
        }
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginModel, string returnUrl = null)
        {
            ViewBag.returnUrl = returnUrl;
            if (!ModelState.IsValid)
                return View();


            ClaimsIdentity identity = null;

            User _user=_context.Users.SingleOrDefault(user => user.UserName.Equals(loginModel.UserName) && user.Password.Equals(loginModel.Password));
            if (_user==null)
            {
                ModelState.AddModelError(String.Empty, "There is no such a user. Please try again.");
                return View();
            }
            else
            {
                //Create the identity for the user
                identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, _user.UserName),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.UserData, _user.Id.ToString())
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                    return RedirectToAction("Index", "Companies");
                else
                    return Redirect(returnUrl);
            }
        }

        [Authorize]
        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
            
        }

        [Authorize]
        public IActionResult AccessDenied()
        {
            
            return View();
        }


    }
}

