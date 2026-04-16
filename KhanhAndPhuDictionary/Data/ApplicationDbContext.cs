using Microsoft.EntityFrameworkCore;
using KhanhAndPhuDictionary.Models;

namespace KhanhAndPhuDictionary.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
    }
}