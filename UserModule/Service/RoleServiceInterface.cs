using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserModule.Dto.Role;

namespace UserModule.Service
{
   public interface RoleServiceInterface
    {
        Task Create(RoleDto dto);
        Task AssingUser(AssignUsersToRoleDto dto);
    }
}
