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
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RourieWebAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserRepository userRepository;


        public AccountController(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            this.userRepository = userRepository;
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
                return View(loginModel);


            ClaimsIdentity identity = null;

            User _user = _context.Users.SingleOrDefault(user => user.UserName.Equals(loginModel.UserName) && user.Password.Equals(loginModel.Password));

            if (_user == null)
            {
                ViewBag.Message = "Please try again...";
                return View(loginModel);
            }
            else
            if (_user.UserType == 1)
            {
                //Create the identity for the admin
                identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, _user.UserName),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.NameIdentifier, _user.Id.ToString()),
                    new Claim(ClaimTypes.UserData, _user.Id.ToString())
                }, CookieAuthenticationDefaults.AuthenticationScheme);
            }
            else
            {
                //Create the identity for normal user
                identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, _user.UserName),
                    new Claim(ClaimTypes.Role, "Normal"),
                    new Claim(ClaimTypes.NameIdentifier, _user.Id.ToString()),
                    new Claim(ClaimTypes.UserData, _user.Id.ToString())
                }, CookieAuthenticationDefaults.AuthenticationScheme);
            }
            var principal = new ClaimsPrincipal(identity);

            var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Companies");
            else
                return Redirect(returnUrl);
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            ChangePasswordModel model = new ChangePasswordModel();
            return View(model);
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
                User user = userRepository.Get(int.Parse(userId));
                //check if there is such a user
                if (user == null)
                    return NotFound();
                
                //check if current password matches
                if (user.Password!=model.CurrentPassword)
                    ViewBag.Message = "Current password does not match!";
                else
                {
                    user.Password = model.Password1;
                    await userRepository.UpdateAsync(user);
                    ViewBag.Message = "Password was changed successfuly";
                }
            }
            else
            {
                foreach (var errorCollection in ModelState.Values)
                {
                    foreach (ModelError error in errorCollection.Errors)
                    {
                        ModelState.AddModelError(String.Empty, error.ErrorMessage);
                    }
                }
            }
            return View(model);
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

