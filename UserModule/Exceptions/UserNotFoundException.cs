using System;
using System.Collections.Generic;
using System.Text;

namespace UserModule.Exceptions
{
 public   class UserNotFoundException:Exception
    {
        public UserNotFoundException(string message ="User Not Found"):base(message)
        {

        }
    }
}
