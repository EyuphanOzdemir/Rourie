using DBAccessLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RourieWebAPI.Controllers;

namespace Controllers
{
    [TestClass]
    public class CompanyControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            CompaniesController controller = new CompaniesController(new MockCompanyRepository(),null, null);
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public void Create()
        {
            // Arrange
            MockCompanyRepository mockCompanyRepository = new MockCompanyRepository();
            CompaniesController controller = new CompaniesController(mockCompanyRepository,null, null);
            int oldCompanyCount = mockCompanyRepository.CountAll();
            // Act
            Company company = new Company() { Id = 3, Address = "TestAddress", Name = "TestName" };
            var result = controller.Create(company);
            // Assert
            int newCompanyCount = mockCompanyRepository.CountAll();
            Assert.AreEqual(newCompanyCount, oldCompanyCount + 1);
        }
    }
}
