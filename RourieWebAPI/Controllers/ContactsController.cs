using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBAccessLibrary;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RourieWebAPI.Models;
using RourieWebAPI.Classes;
using Microsoft.AspNetCore.Authorization;

namespace RourieWebAPI.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private readonly IContactRepository contactRepository;
        private readonly ICompanyRepository companyRepository;
        public ContactsController(IContactRepository contactRepository, ICompanyRepository companyRepository)
        {
            this.contactRepository = contactRepository;
            this.companyRepository = companyRepository;
        }

        // GET and post: contactRepository

        public async Task<IActionResult> Index(ContactListViewModel model)
        {
            model.RowCount =await contactRepository.CountAsync(model.SearchTerm, model.SearchCompanyId);

            if (model.PageId < 1) 
                model.PageId = 1;
            else if (model.PageId > model.GroupCount && model.PageId > 1)
                model.PageId = model.GroupCount;

            
            model.Contacts =contactRepository.Select(model.PageId, model.SearchTerm, model.SearchCompanyId).ToList();
            AddCompanyListToViewBag(model.SearchCompanyId, "All companies");
            return View(model);
        }


        // GET: Contacts/Create
        public IActionResult Create()
        {
            ContactViewModel model = new ContactViewModel(new Contact());
            AddCompanyListToViewBag(0, "Select a company");
            return View(model);
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                await contactRepository.AddAsync(model.contact);
                return RedirectToAction(nameof(Index));
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

                AddCompanyListToViewBag(model.contact.CompanyId);
                return View(model);
            }
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (!contactRepository.Exists(id))
                return NotFound();

            ContactViewModel model = new ContactViewModel(await contactRepository.GetAsync(id));
            AddCompanyListToViewBag(model.contact.CompanyId);
            return View(model);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ContactViewModel model)
        {
            AddCompanyListToViewBag(model.contact.CompanyId);
            if (ModelState.IsValid)
            {
                try
                {
                    await contactRepository.UpdateAsync(model.contact);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await contactRepository.GetAsync(model.contact.Id)==null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.Message = "The contact was updated successfully.";
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

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await contactRepository.GetAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!contactRepository.Exists(id))
                return NotFound();
             contactRepository.Delete(id);
             return RedirectToAction(nameof(Index));
        }

        private bool AddCompanyListToViewBag(int companyID, string selectText="")
        {
            try
            {
                Utility utility = new Utility();
                List<Company> companyList = utility.GetCompanySelectList(companyRepository, selectText);
                ViewBag.CompanySelectList = new SelectList(companyList, "Id", "Name", companyID);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
