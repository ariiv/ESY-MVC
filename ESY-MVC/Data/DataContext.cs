using ESY_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace ESY_MVC.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        { 
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Audit> Audits => Set<Audit>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Seed(modelBuilder);
        }
        private void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "Admin", Password = "Admin", IsAdmin = true },
                new User { Id = 2, Username = "User", Password = "User", IsAdmin = false }
            );
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, ProductName = "HDD 1TB", UnitCount = 55, PricePerUnit = 74.09 },
                new Product { Id = 2, ProductName = "HDD SSD 512GB", UnitCount = 102, PricePerUnit = 190.99 },
                new Product { Id = 3, ProductName = "RAM DDR4 16GB", UnitCount = 47, PricePerUnit = 80.32 }
            );
        }
    }
}
