using System;
using System.Collections.Generic;
using System.Text;

namespace UserModule.Dto.User
{
   public class AssignRoleDto
    {
        public string  UserId { get; set; }
        public IList<RolesDto> Roles { get; set; } = new List<RolesDto>();
    }
    public class RolesDto
    {
        public string Role { get; set; }
        public bool IsSelected { get; set; }
    }

}
