using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DBAccessLibrary
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext dbContext;

        public UserRepository(DataContext context)
        {
            this.dbContext = context;
        }

        public User Add(User user)
        {
            dbContext.Add(user);
            dbContext.SaveChanges();
            return user;
        }

        public int Count()
        {
            return dbContext.Users.Count();
        }

        public User Delete(int id)
        {
            User user = dbContext.Users.Find(id);
            if (user != null)
            {
                dbContext.Remove(user);
                dbContext.SaveChanges();
            }
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return dbContext.Users;
        }

        public User Get(int Id)
        {
            return dbContext.Users.FirstOrDefault(c => c.Id == Id);
        }

        public User Update(User user)
        {
            var e = dbContext.Users.Attach(user);
            e.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            dbContext.SaveChanges();
            return user;
        }


        public bool Exists(int id)
        {
            return Get(id) != null;
        }


        public bool NameExists(string name)
        {
            return dbContext.Users.Any(c => c.UserName == name);
        }

        public bool NameExists(string name, int id)
        {
            return dbContext.Users.Any(c => c.UserName == name && c.Id != id);
        }

        //Async methods
        public async Task<User> AddAsync(User user)
        {
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<int> CountAsync(string search, int selectorId)
        {
            return await dbContext.Users.CountAsync();
        }

        public async Task<User> DeleteAsync(int id)
        {
            User user =await dbContext.Users.FindAsync(id);
            if (user != null)
            {
                dbContext.Remove(user);
                await dbContext.SaveChangesAsync();
            }
            return user;
        }


        public async Task<User> GetAsync(int Id)
        {
            return await dbContext.Users.FirstOrDefaultAsync(c=>c.Id==Id);
        }


        public async Task<User> UpdateAsync(User user)
        {
            var e = dbContext.Users.Attach(user);
            e.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return user;
        }


    }
}
