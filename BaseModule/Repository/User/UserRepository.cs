using BaseModule.BaseRepo;
using BaseModule.DbContextConfig;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModule.Repository;

namespace BaseModule.Repository.User
{
    public class UserRepository:BaseRepository<UserModule.Entity.User>,UserRepositoryInterface
    {
        
        public UserRepository(MyDbContext dbContext):base(dbContext)
        {
                
        }

        public async Task<UserModule.Entity.User> GetByEmail(string email)
        {
            return await GetQueryable().Where(a => a.Email == email).SingleOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<UserModule.Entity.User> GetByMobile(string mobile)
        {
            return await  GetQueryable().Where(a => a.PhoneNumber == mobile).SingleOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<UserModule.Entity.User> GetByUserName(string userName)
        {
            return await GetQueryable().Where(a => a.UserName == userName).SingleOrDefaultAsync().ConfigureAwait(false);
        }
    }
}
