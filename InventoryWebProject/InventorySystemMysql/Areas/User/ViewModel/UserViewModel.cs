using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystemMysql.Areas.User.ViewModel
{

    public class UserIndexViewModel
    {
        public long SNo { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string Status { get; set; }
    }

    public class UserViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
         ErrorMessage = "Invalid email format")]
        public string EmailAddress { get; set; }
        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber,ErrorMessage ="Invalid mobile number")]
        public string MobileNumber { get; set; }
    }

    public class AssignRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IList<RolesViewModel> Roles { get; set; } = new List<RolesViewModel>();
}
public class RolesViewModel
{
    public string  RoleId { get; set; }
    public string  RoleName { get; set; }
    public bool  IsSelected { get; set; }
}

}
