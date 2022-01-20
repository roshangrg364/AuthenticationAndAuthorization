using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserModule.Dto.User;

namespace UserModule.Service
{
  public  interface UserServiceInterface
    {
        Task Create(UserDto dto);
        Task Update(UserDto dto);
        Task Activate(long id);
        Task Deactivate(long id);

       
    }
}
