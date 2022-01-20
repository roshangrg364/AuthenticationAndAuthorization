using InventoryModule.Dto.Category;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InventoryModule.Service
{
   public interface CategoryServiceInterface
    {
        Task Create(CategoryDto dto);
        Task Update(CategoryDto dto);
        Task Delete(int id);
        Task Activate(int id);
        Task Deactivate(int id);
    }
}
