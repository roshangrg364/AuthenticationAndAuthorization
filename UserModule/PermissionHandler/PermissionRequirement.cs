using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserModule.PermissionHandler
{
  public  class PermissionRequirement:IAuthorizationRequirement
    {
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
        public string Permission { get; set; }
    }
}
