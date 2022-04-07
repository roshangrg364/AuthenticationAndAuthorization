using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace AuthenticationAndAuthorization.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="UserName is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; } = "/Home/Index";

        public IList<AuthenticationScheme> ExternalProviders { get; set; } = new List<AuthenticationScheme>();
    }
}
