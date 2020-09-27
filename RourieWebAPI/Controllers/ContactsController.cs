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
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace RourieWebAPI.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private readonly IContactRepository contactRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly ILogger<ContactsController> logger;
        private string UserId { get { return User.FindFirstValue(ClaimTypes.NameIdentifier); } }

        public ContactsController(IContactRepository contactRepository, ICompanyRepository companyRepository, ILogger<ContactsController> logger)
        {
            this.contactRepository = contactRepository;
            this.companyRepository = companyRepository;
            this.logger = logger;
        }

        //GET Contacts/
        public async Task<IActionResult> Index(ContactListViewModel model)
        {
            //the following line can be used to see what will happen when an exception occurs
            //in this case a special exception page should welcome the user (AppError.cshtml)
            //throw new Exception();
            model.RowCount =await contactRepository.CountAsync(model.SearchTerm, model.SearchCompanyId);

            if (model.PageId < 1) model.PageId = 1;
            else if (model.PageId > model.GroupCount && model.PageId > 1)
                model.PageId = model.GroupCount;

            model.Contacts =contactRepository.Select(model.PageId, model.SearchTerm, model.SearchCompanyId).ToList();
            //Add the company list to the view bag
            AddCompanyListToViewBag(model.SearchCompanyId, "All companies");
            int contactCount = model.Contacts.Count();
            
            if (!String.IsNullOrEmpty(model.SearchTerm) || model.SearchCompanyId>0) //if there is a search
            if (contactCount == 0)
                ViewBag.Message = "No contact found!";
            else
                ViewBag.Message = String.Format("{0} contact(s) found",contactCount); //show how many records found
            
            return View(model);
        }


        // GET: Contacts/Create
        public IActionResult Create(int companyId=0)
        {
            int companyCount = companyRepository.CountAll();
            ContactViewModel model = new ContactViewModel(new Contact(),companyCount);
            model.contact.CompanyId = companyId;
            AddCompanyListToViewBag(companyId);
            return View(model);
        }

        // POST: Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (contactRepository.EmailExists(model.contact.Email))
                    ModelState.AddModelError(String.Empty, "There is already a contact with this email");
                else
                if (contactRepository.MobileNumberExists(model.contact.MobileNumber))
                    ModelState.AddModelError(String.Empty, "There is already a contact with this mobile number");
                else
                {
                    //everything is fine
                    await contactRepository.AddAsync(model.contact);
                    TempData["Message"] = "Contact added successfully";
                    logger.LogInformation(String.Format("SPECIAL LOG:The user with id {0} added a contact with name {1}", UserId, "\""+model.contact.Name+"\""));
                    //return to the contact list page
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                //again, if there is an unexpected validation error, show it to the user
                foreach (var errorCollection in ModelState.Values)
                {
                    foreach (ModelError error in errorCollection.Errors)
                    {
                        ModelState.AddModelError(String.Empty, error.ErrorMessage);
                    }
                }
            }
            //if update is not successful for some reason, stay at the same page
            AddCompanyListToViewBag(model.contact.CompanyId);
            return View(model);
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (!contactRepository.Exists(id))
                return NotFound();

            ContactViewModel model = new ContactViewModel(await contactRepository.GetAsync(id), companyRepository.CountAll());
            AddCompanyListToViewBag(model.contact.CompanyId);
            return View(model);
        }

        // POST: Contacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ContactViewModel model)
        {
            //here the user will stay at the same page in any case
            if (ModelState.IsValid)
            {
                try
                {
                    if (contactRepository.EmailExists(model.contact.Email, model.contact.Id))
                        ModelState.AddModelError(String.Empty, "There is already a contact with this email");
                    else
                    if (contactRepository.MobileNumberExists(model.contact.MobileNumber, model.contact.Id))
                        ModelState.AddModelError(String.Empty, "There is already a contact with this mobile number");
                    else
                    {
                        await contactRepository.UpdateAsync(model.contact);
                        ViewBag.Message = "The contact was updated successfully.";
                    }
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
            }
            else
            {
                //show any unexpected validation error to the user
                foreach (var errorCollection in ModelState.Values)
                {
                    foreach (ModelError error in errorCollection.Errors)
                    {
                        ModelState.AddModelError(String.Empty, error.ErrorMessage);
                    }
                }
            }
            AddCompanyListToViewBag(model.contact.CompanyId);
            return View(model);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await contactRepository.GetAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!contactRepository.Exists(id))
                return NotFound();
             contactRepository.Delete(id);
            //Here we cannot use ViewBag because response will be redirected 
            //because of below RedirectToAction
            TempData["Message"] = "Contact deleted";
            return RedirectToAction(nameof(Index));
        }

        //GET Contacts/Details
        public async Task<IActionResult> Details(int id)
        {
            var contact = await contactRepository.GetAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        //Helper method to add company list to the viewbag
        private void AddCompanyListToViewBag(int companyID, string selectText="Select a company")
        {
            Utility utility = new Utility();
            List<Company> companyList = utility.GetCompanySelectList(companyRepository, selectText);
            SelectList companySelectList = new SelectList(companyList, "Id", "Name", companyID);
            //another way of determining the defaul company in the list is as follow
            //companySelectList.FirstOrDefault(item => item.Value == companyID.ToString()).Selected = true;
            ViewBag.CompanySelectList = companySelectList;
        }
    }


}
