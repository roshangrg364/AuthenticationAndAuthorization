using System;
using System.Collections.Generic;
using System.Text;

namespace UserModule.Exceptions
{
   public class RoleNotFoundException:Exception
    {
        public RoleNotFoundException(string message = "role not found"):base(message)
        {

        }
    }
}
