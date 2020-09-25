using DBAccessLibrary;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RourieWebAPI.Classes
{
    public class Utility
    {
        private readonly IWebHostEnvironment hostingEnvironment;

        public Utility(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public Utility()
        {

        }

        public List<Company> GetCompanySelectList(ICompanyRepository companyRepository, string selectText="")
        {
            List<Company> CompanyList = companyRepository.GetAll().ToList();
            if (!String.IsNullOrEmpty(selectText))
            {
                Company selectCompanyOption = new Company(selectText);
                selectCompanyOption.Id = 0;
                CompanyList.Insert(0, selectCompanyOption);
            }
            return CompanyList;
        }
    }
}
