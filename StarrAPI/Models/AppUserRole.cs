using Microsoft.AspNetCore.Identity;

namespace StarrAPI.Models
{
    public class AppUserRole:IdentityUserRole<int>
    {
        public AppUser AppUser { get; set; }
        public AppRole AppRole { get; set; }
    }
}