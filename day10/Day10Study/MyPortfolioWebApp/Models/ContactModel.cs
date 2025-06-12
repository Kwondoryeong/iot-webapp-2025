using System.ComponentModel.DataAnnotations;

namespace MyPortfolioWebApp.Models
{
    public class ContactModel
    {
        [Required(ErrorMessage ="성함은 필수애용")]
        public string Name { get; set; }

        [Required(ErrorMessage = "필수애용")]
        public string Email { get; set; }

        [Required(ErrorMessage = "필수애용")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "필수애용")]
        public string Message { get; set; }
    }
}
