using System;
using System.ComponentModel.DataAnnotations;

namespace StarrAPI.Models
{
    public class Messages
    {
        [Key]
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public string SenderUserName { get; set; }
        public AppUser Sender { get; set; }

        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        public AppUser Recipient { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.Now;
        public bool SenderDelete { get; set; }
        public bool RecipientDelete { get; set; }
    }
}