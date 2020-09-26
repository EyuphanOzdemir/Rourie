using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace DBAccessLibrary
{
    public class MockCompanyRepository : ICompanyRepository
    {
        private List<Company> Companies;

        public MockCompanyRepository()
        {
            Companies = new List<Company>()
            {
                new Company() { Id = 1, Name = "Microsoft", Address = "New York"},
                new Company() { Id = 2, Name = "Google", Address = "California"}
            };
        }

        public Company Get(int Id)
        {
            return this.Companies.FirstOrDefault(e => e.Id == Id);
        }

        public IEnumerable<Company> GetAll()
        {
            return this.Companies;
        }

        public Company Add(Company company)
        {
            company.Id = Companies.Max(e => e.Id) + 1;
            Companies.Add(company);
            return company;
        }

        public Company Update(Company company)
        {
           Company e= Companies.FirstOrDefault(e => e.Id == company.Id);
           if (e!= null)
            {
                e.Name = company.Name;
                e.Address = company.Address;
            }
            return e;
        }

        public Company Delete(int id)
        {
            Company e = Companies.FirstOrDefault(e => e.Id == id);
            if (e != null)
            {
                Companies.Remove(e);
            }
            return e;
        }

        public int Count(string search, int selectorId)
        {
            return Companies.Count(u => (selectorId == 0 || u.Id == selectorId) &&
                                                (String.IsNullOrEmpty(search) || u.Name.Contains(search)));
        }

        public int CountAll()
        {
            return Companies.Count();
        }

        public IEnumerable<Company> Select(int pageID, string search, int selectorId)
        {
            return Companies.Where<Company>(u => String.IsNullOrEmpty(search) || u.Name.Contains(search)
                                           )
                                           .OrderByDescending(u => u.Id)
                                           .Skip((pageID - 1) * 10)
                                           .Take(10)
                                           .ToList();
        }

        public bool Exists(int id)
        {
            return Get(id) != null;
        }

        public bool NameExists(string name)
        {
            return Companies.Any(c => c.Name == name);
        }

        public bool NameExists(string name, int id)
        {
            return Companies.Any(c => c.Name == name && c.Id != id);
        }

        public Task<Company> GetASync(int Id)
        {
            return Task.FromResult(Get(Id));
        }

        public Task<Company> AddAsync(Company company)
        {
            Companies.Add(company);
            return Task.FromResult(company);
        }

        public Task<Company> UpdateAsync(Company company)
        {
            return Task.FromResult(Update(company));
        }

        public Task<Company> DeleteAsync(int id)
        {
            return Task.FromResult(Delete(id));
        }

        public Task<int> CountAsync(string search, int selectorId)
        {
            return Task.FromResult(Count(search,selectorId));
        }
    }
}
