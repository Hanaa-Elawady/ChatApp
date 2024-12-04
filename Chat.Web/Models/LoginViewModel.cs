using System.ComponentModel.DataAnnotations;

namespace Chat.Web.Models
{
    public class LoginViewModel
    {
        [Phone]
        [RegularExpression(@"^(?:\+20|0020)(1[0-9]{9}|2[0-9]{8})$", ErrorMessage = "Please enter a valid Egyptian phone number.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$", ErrorMessage = "Password must have at least one uppercase letter, one lowercase letter, one digit, one non-alphanumeric character, and must be at least 6 characters long.")]
        public string Password { get; set; }
    }
}
