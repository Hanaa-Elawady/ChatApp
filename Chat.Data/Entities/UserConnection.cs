using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Chat.Data.Entities
{
    public class UserConnection
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid User1Id { get; set; }

        [ForeignKey("User1Id")]
        public ApplicationUser User1 { get; set; }

        [Required]
        public Guid User2Id { get; set; }

        [ForeignKey("User2Id")]
        public ApplicationUser User2 { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastMessage { get; set; } = DateTime.UtcNow;

        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}