using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DBAccessLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace RourieWebAPI.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IContactRepository contactRepository;
        private readonly ILogger<CompaniesController> logger;
        //converting identity id to user-friendly string
        //we could do this other places
        private string UserId { get {return User.FindFirstValue(ClaimTypes.NameIdentifier);} }

        //we need both company repository and contact repositort. Besides logging
        public CompaniesController(ICompanyRepository companyRepository, IContactRepository contactRepository, ILogger<CompaniesController> logger)
        {
            this.companyRepository = companyRepository;
            this.contactRepository = contactRepository;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company company)
        {
            if (ModelState.IsValid)
            {
                //check duplicate names
                if (companyRepository.NameExists(company.Name)){
                    ModelState.AddModelError(string.Empty, "There is already a company with this name");
                    return View(company);
                }
                await companyRepository.AddAsync(company);
                //for unit test project try-catch and if (logger!=null) 
                //unit test project has no access to TempData and NLog object
                try { TempData["Message"] = "Company successfuly added"; }
                catch (Exception e)
                {
                    logger.LogError("SPECIAL LOG:TempData could not be set:"+e.Message);
                }
                
                //adding a log saying that this user added a company
                if (logger!=null) 
                    logger.LogInformation(String.Format("SPECIAL LOG:The user with id {0} added a company with name {1}", "\""+UserId,company.Name+"\""));
                //return to Companies/Index
                return RedirectToAction(nameof(Index));
            }
            else //if the state is not valid
            {
                //add any unexpected validation error to validation summary of the view
                foreach (var errorCollection in ModelState.Values)
                {
                    foreach (ModelError error in errorCollection.Errors)
                    {
                        ModelState.AddModelError(String.Empty, error.ErrorMessage);
                    }
                }
                //stay at the same place
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Company company)
        {
            if (ModelState.IsValid) //if there is no validation error
            {
                try
                {
                    //check a duplicate name
                    if (companyRepository.NameExists(company.Name,company.Id))
                        ModelState.AddModelError(string.Empty, "There is already a company with this name");
                    else {
                        await companyRepository.UpdateAsync(company);
                        ViewBag.Message = "Successfuly saved";
                    }
                    //when succeeded, the user stays at the same view, perhaps she will make other changes 
                }
                catch (Exception e)
                {
                    if (!companyRepository.Exists(company.Id))
                    {
                        //this will direct the user to PageNotFound page
                        return NotFound();
                    }
                    else
                    {
                        //if there is such a record, this means that there is a concurrent attempt to update the same record
                        //so fire the exception
                        throw e;
                    }
                }
            } //if the model is not valid for some unknown reason, show it to the user in validation summary
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

            if (contactRepository!=null)
            if (contactRepository.CountByCompany(company.Id) > 0)
                ViewBag.Warning = true;
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
