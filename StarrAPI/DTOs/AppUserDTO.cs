using System;
using System.ComponentModel.DataAnnotations;

namespace StarrAPI.DTOs
{
    public class AppUserDTO
    {
        [Required]
        public string Username { get; set; }  
        [Required]  
        [Display(Name ="Also Known As")]
        public string AlsoKnownAs { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required] 
        [DataType(DataType.Password)]
        // [StringLength(16,ErrorMessage ="Your password is too Long!",MinimumLength = 6,ErrorMessageResourceName ="Your password is too short!")]
        [MinLength(5,ErrorMessage ="Password must be over five characters!")]
        [MaxLength(16,ErrorMessage = "Password is too long! Must be 16 characters but not under five!")]
        public string Password { get; set; }    

    }
}