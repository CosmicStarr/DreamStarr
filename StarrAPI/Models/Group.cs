using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarrAPI.Models
{
    public class Group
    {
        public Group()
        {
            
        }
        public Group(string groupName)
        {
            GroupName = groupName;
        }

        [Key]
        public string GroupName { get; set; }

        public ICollection<Connections> Connections { get; set; } = new List<Connections>();
    }
}