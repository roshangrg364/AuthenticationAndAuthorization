using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModule.BaseRepo
{
   public  interface BaseRepositoryInterface<T> where T : class
    {
        Task<IList<T>> GetAllAsync();
        Task InsertAsync(T entity);
        Task InsertRange(IList<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        IQueryable<T> GetQueryable();
        Task<T> GetById(long id);
    }
}
