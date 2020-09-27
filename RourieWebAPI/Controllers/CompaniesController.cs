using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DBAccessLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace RourieWebAPI.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {
        private readonly ICompanyRepository companyRepository;
        private readonly ILogger<CompaniesController> logger;
        private string UserId { get {return User.FindFirstValue(ClaimTypes.NameIdentifier);} }

        public CompaniesController(ICompanyRepository companyRepository, ILogger<CompaniesController> logger)
        {
            this.companyRepository = companyRepository;
            this.logger = logger;
        }


        // GET: Companies
        public IActionResult Index()
        {
            var companyList = companyRepository.GetAll();
            return View(companyList);
        }

        // GET: Companies/Details/5
        public async  Task<IActionResult> Details(int id)
        {
            var company = await companyRepository.GetASync(id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company company)
        {
            if (ModelState.IsValid)
            {
                if (companyRepository.NameExists(company.Name)){
                    ModelState.AddModelError(string.Empty, "There is already a company with this name");
                    return View(company);
                }
                await companyRepository.AddAsync(company);
                //for unit test project try-catch and if (logger!=null) 
                //unit test project has no access to TempData and NLog object
                try { TempData["Message"] = "Company successfuly added"; } catch(Exception e) {  };
                if (logger!=null) 
                    logger.LogInformation(String.Format("==============The user with id {0} added a company with name {1}==============", UserId,company.Name));
                //
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
                return View(company);
            }
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var company = await companyRepository.GetASync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (companyRepository.NameExists(company.Name,company.Id))
                        ModelState.AddModelError(string.Empty, "There is already a company with this name");
                    else {
                        await companyRepository.UpdateAsync(company);
                        ViewBag.Message = "Successfuly saved";
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!companyRepository.Exists(company.Id))
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
                foreach (var errorCollection in ModelState.Values)
                {
                    foreach (ModelError error in errorCollection.Errors)
                    {
                        ModelState.AddModelError(String.Empty, error.ErrorMessage);
                    }
                }
            }
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var company = await companyRepository.GetASync(id);
            if (company == null)
                return NotFound();
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!companyRepository.Exists(id))
                return NotFound();

            companyRepository.Delete(id);
            TempData["Message"] = "The company deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
