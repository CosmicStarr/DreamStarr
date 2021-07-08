using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StarrAPI.Extensions;

namespace StarrAPI.Models
{
    public class AppUser
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }        
        public byte[] PasswordHash { get; set; }        
        public byte[] PasswordSalt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AlsoKnownAs { get; set; } 
        public DateTime ProfileCreated { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string InterestedIn { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photos> Photos { get; set; }
        //List of Users that Liked the Current User
        public ICollection<UserLikes> LikedByUsers {get; set;}
        //List of Users that the Current User Likes
        public ICollection<UserLikes> LikedUsers { get; set; }

    }
}