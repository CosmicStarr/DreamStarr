using System.ComponentModel.DataAnnotations;

namespace StarrAPI.DTOs
{
    public class AppUserDTO
    {
        [Required]
        public string Username { get; set; }    
        [Required] 
        [DataType(DataType.Password)]
        [MinLength(5,ErrorMessage ="Password must be over five characters!")]
        [MaxLength(16,ErrorMessage = "Password is too long! Must be 16 characters but not under five!")]
        public string Password { get; set; }     
    }
}