using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace UserModule.TransactionScopeConfig
{
 public   class TransactionScopeHelper
    {
        public static TransactionScope GetInstance()
        {
            return new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}
