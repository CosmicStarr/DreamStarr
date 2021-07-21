using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StarrAPI.Models;

namespace StarrAPI.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if(await userManager.Users.AnyAsync()) return;
            
                var UserData = await System.IO.File.ReadAllTextAsync("Data/SeedDataForUser.json");
                var User = JsonSerializer.Deserialize<List<AppUser>>(UserData);
                if(User == null) return;
                
                var Roles = new List<AppRole>
                {
                    new AppRole{Name ="Admin"},
                    new AppRole{Name ="Member"},
                    new AppRole{Name ="Manager"}
                };

                foreach (var item in Roles)
                {
                    await roleManager.CreateAsync(item);
                }

                foreach (var item in User)
                {   
                    item.UserName = item.UserName.ToLower();
                    await userManager.CreateAsync(item,"Sonics@123");
                    await userManager.AddToRoleAsync(item,"Member");
                }

                var admin = new AppUser
                {
                    UserName = "Admin"
                };

                await userManager.CreateAsync(admin,"Sonics@123");
                await userManager.AddToRolesAsync(admin, new[] {"Admin","Manager"});
           
        }
    }
}