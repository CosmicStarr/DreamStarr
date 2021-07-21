using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace StarrAPI.Models
{
    public class AppUser:IdentityUser<int>
    {
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
        public ICollection<Messages> MessagesSent { get; set; }
        public ICollection<Messages> MessagesReceived { get; set; }
        public ICollection<AppUserRole> ListOfAppUserRole { get; set; }

    }
}