
using InventoryModule.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryModule.Repository
{
   public interface CategoryRepositoryInterface
    {
        Task<Category?> GetByName(string name);
        Task<IList<Category>> GetAllAsync();
        Task InsertAsync(Category entity);
        Task InsertRange(IList<Category> entities);
        Task UpdateAsync(Category entity);
        Task DeleteAsync(Category entity);
        IQueryable<Category> GetQueryable();
        Task<Category> GetById(long id);
    }
}
