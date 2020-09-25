using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DBAccessLibrary
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DataContext dbContext;

        public CompanyRepository(DataContext context)
        {
            this.dbContext = context;
        }

        public Company Add(Company company)
        {
            dbContext.Add(company);
            dbContext.SaveChanges();
            return company;
        }

        public int Count(string search, int selectorId)
        {
            return dbContext.Companies.Count(u => (selectorId == 0 || u.Id == selectorId) &&
                                                (String.IsNullOrEmpty(search) || u.Name.Contains(search)));
        }

        public Company Delete(int id)
        {
            Company company = dbContext.Companies.Find(id);
            if (company != null)
            {
                dbContext.Remove(company);
                dbContext.SaveChanges();
            }
            return company;
        }

        public IEnumerable<Company> GetAll()
        {
            return dbContext.Companies;
        }

        public Company Get(int Id)
        {
            return dbContext.Companies.Find(Id);
        }

        public IEnumerable<Company> Select(int pageID, string search, int selectorId)
        {
            return dbContext.Companies.Where<Company>(u => String.IsNullOrEmpty(search) || u.Name.Contains(search)
                                           )
                                           .OrderByDescending(u => u.Id)
                                           .Skip((pageID - 1) * 10)
                                           .Take(10)
                                           .ToList();
        }

        public Company Update(Company company)
        {
            var e = dbContext.Companies.Attach(company);
            e.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            dbContext.SaveChanges();
            return company;
        }


        public bool Exists(int id)
        {
            return Get(id) != null;
        }

        //Async methods
        public async Task<Company> AddAsync(Company company)
        {
            await dbContext.AddAsync(company);
            await dbContext.SaveChangesAsync();
            return company;
        }

        public async Task<int> CountAsync(string search, int selectorId)
        {
            return await dbContext.Companies.CountAsync(u => (selectorId == 0 || u.Id == selectorId) &&
                                                (String.IsNullOrEmpty(search) || u.Name.Contains(search)));
        }

        public async Task<Company> DeleteAsync(int id)
        {
            Company company =await dbContext.Companies.FindAsync(id);
            if (company != null)
            {
                dbContext.Remove(company);
                await dbContext.SaveChangesAsync();
            }
            return company;
        }


        public async Task<Company> GetASync(int Id)
        {
            return await dbContext.Companies.FindAsync(Id);
        }


        public async Task<Company> UpdateAsync(Company company)
        {
            var e = dbContext.Companies.Attach(company);
            e.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return company;
        }

    }
}
