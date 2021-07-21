using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using StarrAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace StarrAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser,AppRole,int,
    IdentityUserClaim<int>,AppUserRole,IdentityUserLogin<int>,IdentityRoleClaim<int>,IdentityUserToken<int>>
    {  
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

   
        public DbSet<UserLikes> Likes { get; set; }
        public DbSet<Messages> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
            .HasMany(r => r.ListOfAppUserRole)
            .WithOne(r => r.AppUser)
            .HasForeignKey(r =>r.UserId)
            .IsRequired();

            builder.Entity<AppRole>()
            .HasMany(r => r.ListOfAppUserRole)
            .WithOne(r => r.AppRole)
            .HasForeignKey(r =>r.RoleId)
            .IsRequired();

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


            builder.Entity<Messages>()
            .HasOne(u =>u.Recipient)
            .WithMany(m => m.MessagesReceived)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Messages>()
            .HasOne(u =>u.Sender)
            .WithMany(m => m.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);
            
        }
    }
}