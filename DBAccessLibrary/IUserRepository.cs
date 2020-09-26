using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBAccessLibrary
{
    public interface IUserRepository
    {
        public User Get(int Id);
        public IEnumerable<User> GetAll();
        public User Add(User user);
        public User Update(User user);
        public User Delete(int id);
        public int Count();
        public bool Exists(int id);

        public bool NameExists(string name);
        public bool NameExists(string name, int id);

        //async methods
        public Task<User> GetAsync(int Id);
        public Task<User> AddAsync(User user);
        public Task<User> UpdateAsync(User user);
        public Task<User> DeleteAsync(int id);
        public Task<int> CountAsync(string search, int selectorId);
        
    }
}
