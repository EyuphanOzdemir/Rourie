using Microsoft.EntityFrameworkCore;
using CommonLibrary;

namespace DBAccessLibrary
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DataContext()
        {

        }

        //EntityEntry<TEntity> Update([NotNullAttribute] TEntity entity);

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Common.ReadFromConfig("ConnectionStrings", "DBConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //User table
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>().HasIndex(e => e.UserName).IsUnique();
            modelBuilder.Entity<User>().HasData(new User {Id=1, UserName="admin", Password="admin123", UserType=1 });
            modelBuilder.Entity<User>().HasData(new User { Id = 2, UserName = "testuser", Password = "testuser123", UserType = 0 });
            //Company table
            modelBuilder.Entity<Company>().ToTable("Company");
            modelBuilder.Entity<Company>().HasIndex(e => e.Name).IsUnique();
            //Contact table
            modelBuilder.Entity<Contact>().ToTable("Contact");
            modelBuilder.Entity<Contact>().HasIndex(e => e.Email);

        }
    }
}
