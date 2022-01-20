using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModule.Entity;

namespace UserModule.Repository
{
  public  interface UserRepositoryInterface
    {
        Task<User?> GetByMobile(string mobile);
        Task<User?> GetByEmail(string email);
        Task<User?> GetByUserName(string name);
        Task<IList<User>> GetAllAsync();
        Task InsertAsync(User entity);
        Task InsertRange(IList<User> entities);
        Task UpdateAsync(User entity);
        Task DeleteAsync(User entity);
        IQueryable<User> GetQueryable();
        Task<User> GetById(long id);
    }
}
