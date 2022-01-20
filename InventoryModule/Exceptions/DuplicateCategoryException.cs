using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryModule.Exceptions
{
public    class DuplicateCategoryException:Exception
    {
        public DuplicateCategoryException(string message = "Duplicate Category"):base(message)
        {

        }
    }
}
