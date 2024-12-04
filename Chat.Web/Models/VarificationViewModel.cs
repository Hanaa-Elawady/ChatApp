using System.ComponentModel.DataAnnotations;

namespace Chat.Web.Models
{
    public class VarificationViewModel
    {
        [Phone]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Wrong Code,Please enter The Varification Code sent to your phone Number ..")]
        public string Code { get; set; }
    }
}
