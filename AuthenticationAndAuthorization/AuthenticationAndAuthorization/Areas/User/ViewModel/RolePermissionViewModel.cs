using System.Collections.Generic;

namespace AuthenticationAndAuthorization.Areas.User.ViewModel
{
    public class RolePermissionViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public IList<AssignPermissionViewModel> Permissions { get; set; } = new List<AssignPermissionViewModel>();
    }
    public class AssignPermissionViewModel
    {
        public string Permission { get; set; }
        public bool IsSelected { get; set; }
    }
}
