using System.ComponentModel.DataAnnotations;

namespace StarrAPI.Models
{
    public class AppUser
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }        
        public byte[] PasswordHash { get; set; }        
        public byte[] PasswordSalt { get; set; }        
    }
}