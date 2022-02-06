using System;
using System.Collections.Generic;
using System.Text;

namespace UserModule.Dto.Role
{
   public  class AssignUsersToRoleDto
    {
        public string RoleId { get; set; }
        public IList<AssignUserDto> Users { get; set; } = new List<AssignUserDto>();
    }

    public class AssignUserDto
    {
        public string UserId { get; set; }
        public bool IsSelected { get; set; }
    }
}
