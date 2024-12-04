using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Data.Entities
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ConnectionId { get; set; }

        [ForeignKey("ConnectionId")]
        public UserConnection Connection { get; set; }

        [Required]
        public Guid SenderId { get; set; }

        [ForeignKey("SenderId")]
        public ApplicationUser Sender { get; set; }

        public bool Seen { get; set; } = false;

        public DateTime DateSent { get; set; } = DateTime.UtcNow;

        public DateTime? DateSeen { get; set; }

        public string Text { get; set; }

        public string? AttachmentUrl { get; set; }

        public string? AttachmentType { get; set; }
    }
}