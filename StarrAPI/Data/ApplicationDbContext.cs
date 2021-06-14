using Microsoft.EntityFrameworkCore;
using StarrAPI.Models;

namespace StarrAPI.Data
{
    public class ApplicationDbContext : DbContext
    {  
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<AppUser> GetAppUsers { get; set; } 
    }
}