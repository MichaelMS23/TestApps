using Microsoft.EntityFrameworkCore;
using TestApps.Models; // ✅ IMPORTANT!

namespace TestApps.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; } // ✅ This Product must match the using above
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
