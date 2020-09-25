using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBAccessLibrary
{
    public interface IContactRepository
    {
        public Contact Get(int Id);
        public IEnumerable<Contact> GetAll();
        public Contact Add(Contact contact);
        public Contact Update(Contact contact);
        public Contact Delete(int id);
        public int Count(string search, int selectorId);
        public IEnumerable<Contact> Select(int pageID, string search, int selectorId);
        public bool Exists(int id);
        //async methods
        public Task<Contact> GetAsync(int Id);
        public Task<Contact> AddAsync(Contact contact);
        public Task<Contact> UpdateAsync(Contact contact);
        public Task<Contact> DeleteAsync(int id);
        public Task<int> CountAsync(string search, int selectorId);
        
    }
}
