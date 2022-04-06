using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystemMysql.Areas.User.ViewModel
{
    public class AddPasswordViewModel
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword", ErrorMessage = "password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
