using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarrAPI.Models;

namespace StarrAPI.Data
{
    public class Seed
    {
        public static async Task SeedUsers(ApplicationDbContext Context)
        {
            if(await Context.GetAppUsers.AnyAsync()) return;
            
            var UserData = await System.IO.File.ReadAllTextAsync("Data/SeedDataForUser.json");
            var User = JsonSerializer.Deserialize<List<AppUser>>(UserData);
            foreach (var item in User)
            {
                using var Hmac = new HMACSHA512();
                item.Username = item.Username.ToLower();
                item.PasswordHash = Hmac.ComputeHash(Encoding.UTF8.GetBytes("Sonics@123"));
                item.PasswordSalt = Hmac.Key;
                Context.GetAppUsers.Add(item);
            }
            await Context.SaveChangesAsync();
        }
    }
}