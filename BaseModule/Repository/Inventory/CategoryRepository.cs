using BaseModule.BaseRepo;
using BaseModule.DbContextConfig;
using InventoryModule.Entity;
using InventoryModule.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModule.Repository.Inventory
{
    public class CategoryRepository : BaseRepository<Category>, CategoryRepositoryInterface

    {
        public CategoryRepository(MyDbContext context) : base(context)
        {

        }

        public async Task<Category?> GetByName(string name)
        {
            return await GetQueryable().Where(a => a.Name == name).SingleOrDefaultAsync().ConfigureAwait(false);
        }

      
    }
}
