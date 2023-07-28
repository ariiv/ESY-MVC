using ESY_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace ESY_MVC.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        { 
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("DefaultConnection", builder =>
        //    {
        //        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        //    });
        //    base.OnConfiguring(optionsBuilder);
        //}
        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Audit> Audits => Set<Audit>();
    }
}
