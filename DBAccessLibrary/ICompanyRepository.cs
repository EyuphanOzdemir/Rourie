using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBAccessLibrary
{
    public interface ICompanyRepository
    {
        public Company Get(int Id);
        public IEnumerable<Company> GetAll();
        public Company Add(Company company);
        public Company Update(Company company);
        public Company Delete(int id);
        public int Count(string search, int selectorId);

        public int CountAll();
        public IEnumerable<Company> Select(int pageID, string search, int selectorId);
        public bool Exists(int id);
        public bool NameExists(string name);
        public bool NameExists(string name, int id);

        //async methods
        public Task<Company> GetASync(int Id);
        public Task<Company> AddAsync(Company company);
        public Task<Company> UpdateAsync(Company company);
        public Task<Company> DeleteAsync(int id);
        public Task<int> CountAsync(string search, int selectorId);
        
    }
}
