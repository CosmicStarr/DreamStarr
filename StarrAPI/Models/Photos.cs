using System.ComponentModel.DataAnnotations.Schema;

namespace StarrAPI.Models
{
    [Table("Pictures")]
    public class Photos
    {
 
        public int ID { get; set; }
        public string PhotoUrl { get; set; }
        public bool MainPic { get; set; }
        public string PublicId { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserID { get; set; }

    }
}