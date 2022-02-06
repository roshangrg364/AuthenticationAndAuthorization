using BaseModule.BaseRepo;
using BaseModule.DbContextConfig;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using UserModule.Repository;

namespace BaseModule.Repository.User
{
  public  class RoleRepository:BaseRepository<IdentityRole>, RoleRepositoryInterface
    {
        public RoleRepository(MyDbContext dbContext) : base(dbContext)
        {

        }

    }
}
