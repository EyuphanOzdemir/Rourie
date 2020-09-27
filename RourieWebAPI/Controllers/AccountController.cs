using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RourieWebAPI.Models;
using DBAccessLibrary;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RourieWebAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserRepository userRepository;


        //setting dbcontext and user repository via dependeceny injection
        public AccountController(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            this.userRepository = userRepository;
        }
        
        //Get (if the user is not logged in firstly login is wanted, then when login is successful, the user 
        //is directed to the resource that she wanted in the first place)
        //GET Account/Login
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        //POST Account/Login with LoginModel object 
        public IActionResult Login(LoginViewModel loginModel, string returnUrl = null)
        {
            ViewBag.returnUrl = returnUrl;
            if (!ModelState.IsValid)
                return View(loginModel);


            ClaimsIdentity identity = null;
            //login check
            User _user = _context.Users.SingleOrDefault(user => user.UserName.Equals(loginModel.UserName) && user.Password.Equals(loginModel.Password));
            //if there is no such a user...
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

            //write cookie
            var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            //here the user is directed to the view she wanted to go in the beginning
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Companies");
            else
                return Redirect(returnUrl);
        }


        [HttpGet]
        //GET Account/ChangePassword
        public IActionResult ChangePassword()
        {
            ChangePasswordModel model = new ChangePasswordModel();
            return View(model);
        }

        // POST: Account/ChangePassword
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
                    //everything is fine
                    //the user stays at the same page with a success message
                    user.Password = model.Password1;
                    await userRepository.UpdateAsync(user);
                    ViewBag.Message = "Password was changed successfuly";
                }
            }
            else
            {
                //if any unexpected validation error, show it in the validation summary
                foreach (var errorCollection in ModelState.Values)
                {
                    foreach (ModelError error in errorCollection.Errors)
                    {
                        ModelState.AddModelError(String.Empty, error.ErrorMessage);
                    }
                }
            }
            //return to the same view
            return View(model);
        }




        [Authorize]
        //only logged-in user can do this
        //GET Account/Logout
        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
            
        }

        [Authorize]
        //any authorization violation comes here because 
        //in startup.cs, when we add authorization middleware we set this
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}

