using System.ComponentModel.DataAnnotations;

namespace StarrAPI.Models
{
    public class Connections
    {
        public Connections()
        {
            
        }
        public Connections(string connectionId, string username)
        {
            ConnectionId = connectionId;
            Username = username;
        }
        [Key]
        public string ConnectionId { get; set; }
        public string Username { get; set; }
    }
}