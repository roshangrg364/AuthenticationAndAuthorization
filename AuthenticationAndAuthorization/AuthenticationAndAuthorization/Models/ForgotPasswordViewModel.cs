using System.ComponentModel.DataAnnotations;

namespace AuthenticationAndAuthorization.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
