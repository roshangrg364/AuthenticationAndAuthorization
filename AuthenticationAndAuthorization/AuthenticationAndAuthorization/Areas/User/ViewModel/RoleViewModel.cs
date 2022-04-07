using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationAndAuthorization.Areas.User.ViewModel
{
    public class RoleIndexViewModel
    {
        public long Sno { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class RoleViewModel
    {
        [Required(ErrorMessage ="Role is required")]
        public string RoleName { get; set; }
    }
    public class RoleEditViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class AssignRoleToUserIndexViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public IList<AssignRoleToUserViewModel> Users { get; set; } = new List<AssignRoleToUserViewModel>();
    }

    public class AssignRoleToUserViewModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
