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

namespace RourieWebAPI.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index(int pageId = 1, string search = "")
        {
            UserViewModel model = new UserViewModel();
            model.PageId = pageId;
            model.SearchTerm = search;
            

            bool doSearch = !String.IsNullOrEmpty(model.SearchTerm) && model.SearchTerm.Length >= 3;

            Func<User, bool> searchPredicate = (u => (u.UserName + u.Email).Contains(model.SearchTerm));
            Func<User, bool> totalPredicate = (u => (!doSearch || searchPredicate(u)));

            model.RowCount = _context.Users.Count(u => totalPredicate(u));

            if (model.PageId < 1) model.PageId = 1;
            else if (model.PageId > model.GroupCount && model.PageId > 1) model.PageId = model.GroupCount;

            IQueryable<User> users = null;

            users = _context.Users.Where(u => totalPredicate(u)).Skip((pageId - 1) * 10).Take(10);
            model.Users = await users.ToListAsync();

            return View(model);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private void DeleteUser(int userId)
        {
            User user = _context.Users.SingleOrDefault(u => u.Id == userId);
            _context.SaveChanges();
        }


        #region "Delete"
        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
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
            DeleteUser(id);
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
