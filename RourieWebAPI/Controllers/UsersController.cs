using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBAccessLibrary;
using Microsoft.AspNetCore.Authorization;
using RourieWebAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using RourieWebAPI.Classes;

namespace RourieWebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        // GET: Users
        public IActionResult Index()
        {
            return View(userRepository.GetAll().ToList());
        }

        public IActionResult Create()
        {
            AddUserTypeListToViewBag();
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserAddModel model)
        {
            if (ModelState.IsValid)
            {
                if (userRepository.NameExists(model.UserName))
                    ModelState.AddModelError(string.Empty, "There is already a user with this user name");
                else
                {
                    User user = new User();
                    user.UserName = model.UserName;
                    user.Password = model.Password1;
                    user.UserType = model.UserType;
                    await userRepository.AddAsync(user);
                    TempData["Message"] = "User successfuly added";
                    return RedirectToAction(nameof(Index));
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
            AddUserTypeListToViewBag();
            return View(model);
        }


 

        #region "Delete"
        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var user = await userRepository.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            User user = userRepository.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            else if (user.UserType == 1) //admin user
                TempData["Message"] = "You attempted to delete an admin, which is impossible!";
            else
            {
                userRepository.Delete(id);
                TempData["Message"] = "The user was successfuly deleted";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AddUserTypeListToViewBag()
        {
            try
            {
                UserType type1 = new UserType("Normal",0);
                UserType type2 = new UserType("Admin", 1);
                List<UserType> list = new List<UserType>(){type1,type2};
                SelectList userTypeList = new SelectList(list, "Value", "Text", 0);
                ViewBag.UserTypeList = userTypeList;
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
