using System;
using System.Collections.Generic;

namespace StarrAPI.DTOs
{
    public class MemberDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } 
        public string PhotoUrl { get; set; }       
        public int Age { get; set; }
        public string AlsoKnownAs { get; set; } 
        public DateTime ProfileCreated { get; set; } 
        public DateTime LastActive { get; set; } 
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string InterestedIn { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<PhotosDTO> Photos { get; set; }
    }
}