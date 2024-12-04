using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Chat.Data.Entities
{
    public class ApplicationUser :IdentityUser<Guid>
    {
        [Key]
        public override Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public string ProfilePicture { get; set; }

        public string Status { get; set; } = string.Empty;
        public ICollection<UserConnection> Contacts { get; set; } = new List<UserConnection>();

    }
}
