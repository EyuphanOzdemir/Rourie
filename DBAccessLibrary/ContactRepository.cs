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
            return dbContext.Contacts.Count(u => (selectorId == 0 || u.Id == selectorId) &&
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
            return dbContext.Contacts.Find(Id);
        }

        public IEnumerable<Contact> Select(int pageID, string search, int selectorId)
        {
            return dbContext.Contacts.Include(e => e.Company).Where<Contact>(u => String.IsNullOrEmpty(search) || u.Name.Contains(search)
                                           )
                                           .OrderByDescending(u => u.Id)
                                           .Skip((pageID - 1) * 10)
                                           .Take(10)
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

        //Async methods
        public async Task<Contact> AddAsync(Contact contact)
        {
            await dbContext.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return contact;
        }

        public async Task<int> CountAsync(string search, int selectorId)
        {
            return await dbContext.Contacts.CountAsync(u => (selectorId == 0 || u.Id == selectorId) &&
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
            return await dbContext.Contacts.FindAsync(Id);
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
