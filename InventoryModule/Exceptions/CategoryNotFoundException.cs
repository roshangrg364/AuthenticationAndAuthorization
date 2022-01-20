using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryModule.Exceptions
{
   public class CategoryNotFoundException:Exception
    {
        public CategoryNotFoundException(string message = "Category Not Found"):base(message)
        {

        }
    }
}
