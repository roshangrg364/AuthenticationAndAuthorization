using System;
using System.Collections.Generic;
using System.Text;

namespace UserModule.PermissionHandler
{
   public class Permission
    {
        public const string PermissionClaimType = "Permission";

        public static IList<string> Permissions = new List<string>
        {
            "User.View",
            "User.Create",
            "User.Edit",
            "User.AssignRole",
            "User.AssignPermission",
            "Role.View",
            "Role.Create"
        };
    }
}
