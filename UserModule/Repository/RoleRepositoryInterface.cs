using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserModule.Repository
{
  public  interface RoleRepositoryInterface
    {
        Task<IList<IdentityRole>> GetAllAsync();
     
        IQueryable<IdentityRole> GetQueryable();
       

    }
}
