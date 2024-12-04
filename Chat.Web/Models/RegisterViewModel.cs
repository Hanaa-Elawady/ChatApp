using System.ComponentModel.DataAnnotations;

namespace Chat.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]

        public string DisplayName { get; set; }
        public IFormFile? ProfilePicture { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$", ErrorMessage = "Password must have at least one uppercase letter, one lowercase letter, one digit, one non-alphanumeric character, and must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
