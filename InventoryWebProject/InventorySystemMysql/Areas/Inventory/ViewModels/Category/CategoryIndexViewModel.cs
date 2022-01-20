using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystemMysql.Areas.Inventory.ViewModels.Category
{
    public class CategoryIndexViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }
}
