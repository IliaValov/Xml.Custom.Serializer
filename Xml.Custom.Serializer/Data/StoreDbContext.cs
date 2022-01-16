using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions options) : base(options)
        {
        }

        public StoreDbContext()
        {
        }

        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=StoreDb;Username=postgres;Password=_Passw0rd;");
        }
    }
}
