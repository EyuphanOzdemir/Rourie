using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DBAccessLibrary
{
    public class ContactRepository : IContactRepository
    {
        private readonly DataContext dbContext;

        public ContactRepository(DataContext context)
        {
            this.dbContext = context;
        }

        public Contact Add(Contact contact)
        {
            dbContext.Add(contact);
            dbContext.SaveChanges();
            return contact;
        }

        public int Count(string search, int selectorId)
        {
            return dbContext.Contacts.Count(u => (selectorId == 0 || u.CompanyId == selectorId) &&
                                                (String.IsNullOrEmpty(search) || u.Name.Contains(search)));
        }

        public Contact Delete(int id)
        {
            Contact contact = dbContext.Contacts.Find(id);
            if (contact != null)
            {
                dbContext.Remove(contact);
                dbContext.SaveChanges();
            }
            return contact;
        }

        public IEnumerable<Contact> GetAll()
        {
            return dbContext.Contacts;
        }

        public Contact Get(int Id)
        {
            return dbContext.Contacts.Include(e => e.Company).FirstOrDefault(c => c.Id == Id);
        }

        public IEnumerable<Contact> Select(int pageID,string search, int selectorId)
        {
            return dbContext.Contacts.Where<Contact>(u => (selectorId == 0 || u.CompanyId == selectorId) &&
                                               (String.IsNullOrEmpty(search) || u.Name.Contains(search))
                                           )
                                           .OrderByDescending(u => u.Id)
                                           .Skip((pageID - 1) * 10)
                                           .Take(10)
                                           .Include(e => e.Company)
                                           .ToList();
        }

        public Contact Update(Contact contact)
        {
            var e = dbContext.Contacts.Attach(contact);
            e.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            dbContext.SaveChanges();
            return contact;
        }


        public bool Exists(int id)
        {
            return Get(id) != null;
        }


        public bool EmailExists(string email)
        {
            return dbContext.Contacts.Any(c => c.Email == email);
        }

        public bool EmailExists(string email, int exceptContactId)
        {
            return dbContext.Contacts.Any(c => c.Email == email && c.Id != exceptContactId);
        }

        public bool MobileNumberExists(string mobileNumber)
        {
            return dbContext.Contacts.Any(c => c.MobileNumber == mobileNumber);
        }

        public bool MobileNumberExists(string mobileNumber, int exceptContactId)
        {
            return dbContext.Contacts.Any(c => c.MobileNumber == mobileNumber && c.Id != exceptContactId);
        }

        public int CountByCompany(int companyId)
        {
            return dbContext.Contacts.Count(c => c.CompanyId == companyId);
        }

        //Async methods
        public async Task<Contact> AddAsync(Contact contact)
        {
            await dbContext.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return contact;
        }

        public async Task<int> CountAsync(string search, int selectorId)
        {
            return await dbContext.Contacts.CountAsync(u => (selectorId == 0 || u.CompanyId== selectorId) &&
                                               (String.IsNullOrEmpty(search) || u.Name.Contains(search)));
        }

        public async Task<Contact> DeleteAsync(int id)
        {
            Contact contact =await dbContext.Contacts.FindAsync(id);
            if (contact != null)
            {
                dbContext.Remove(contact);
                await dbContext.SaveChangesAsync();
            }
            return contact;
        }


        public async Task<Contact> GetAsync(int Id)
        {
            return await dbContext.Contacts.Include(e => e.Company).FirstOrDefaultAsync(c=>c.Id==Id);
        }


        public async Task<Contact> UpdateAsync(Contact contact)
        {
            var e = dbContext.Contacts.Attach(contact);
            e.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return contact;
        }


    }
}
