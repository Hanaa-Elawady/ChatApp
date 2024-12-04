using System.ComponentModel.DataAnnotations;

namespace Chat.Web.Models
{
    public class CheckPhoneNumberViewModel
    {
        [Phone]
        [RegularExpression(@"^(?:\+20|0020)(1[0-9]{9}|2[0-9]{8})$", ErrorMessage = "Please enter a valid Egyptian phone number.")]
        public string PhoneNumber { get; set; }

    }
}
