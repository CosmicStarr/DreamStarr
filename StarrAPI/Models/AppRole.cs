using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace StarrAPI.Models
{
    public class AppRole:IdentityRole<int>
    {
        public ICollection<AppUserRole> ListOfAppUserRole { get; set; }
    }
}