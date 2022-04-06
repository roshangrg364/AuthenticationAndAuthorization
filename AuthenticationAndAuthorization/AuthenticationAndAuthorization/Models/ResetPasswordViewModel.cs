using System.ComponentModel.DataAnnotations;

namespace AuthenticationAndAuthorization.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Token { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password",ErrorMessage ="Password do not match")]
        public string  ConfirmPassword { get; set; }
    }
}
