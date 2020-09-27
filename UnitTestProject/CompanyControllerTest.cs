using DBAccessLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RourieWebAPI.Controllers;


//there are only two test methods.
//There should be many of course.
//However, these samples can be followed for the others
namespace Controllers
{
    [TestClass]
    public class CompanyControllerTest
    {
        [TestMethod]
        //test whether Companies/Index produces a view
        public void Index()
        {
            // Arrange
            CompaniesController controller = new CompaniesController(new MockCompanyRepository(),null, null);
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
            //this test is successful
        }

        [TestMethod]
        //check a company is really added (test Companies/Create)
        public void Create()
        {
            // Arrange
            MockCompanyRepository mockCompanyRepository = new MockCompanyRepository();
            CompaniesController controller = new CompaniesController(mockCompanyRepository,null, null);
            int oldCompanyCount = mockCompanyRepository.CountAll();
            // Act
            //since we are working with in-memory list of companies, who cares Id...
            Company company = new Company() { Id = -1, Address = "TestAddress", Name = "TestName" };
            var result = controller.Create(company);
            // Assert
            int newCompanyCount = mockCompanyRepository.CountAll();
            Assert.AreEqual(newCompanyCount, oldCompanyCount + 1);
            //this test is successful
        }
    }
}
