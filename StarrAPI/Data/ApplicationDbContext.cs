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
        public DbSet<UserLikes> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLikes>()
            .HasKey(k => new{ k.SourceUserId,k.LikedUserId});

            builder.Entity<UserLikes>()
            .HasOne(u => u.SourceUser)
            .WithMany(w => w.LikedUsers)
            .HasForeignKey(f => f.SourceUserId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserLikes>()
            .HasOne(u => u.LikedUser)
            .WithMany(w => w.LikedByUsers)
            .HasForeignKey(f => f.LikedUserId)
            .OnDelete(DeleteBehavior.NoAction);
            
        }
    }
}