using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModule.Entity;
using UserModule.Exceptions;

namespace UserModule.PermissionHandler
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public PermissionAuthorizationHandler(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
            {
                throw new UserNotFoundException();
            }

            // Get all the roles the user belongs to and check if any of the roles has the permission required
            // for the authorization to succeed.
            var user = await _userManager.GetUserAsync(context.User);
            if (user.Type == UserModule.Entity.User.TypeSuperAdmin || user.Type == UserModule.Entity.User.TypeExternal)
            {
                context.Succeed(requirement);
                return;
            }
            var userRoleNames = await _userManager.GetRolesAsync(user);
            var userRoles = _roleManager.Roles.Where(x => userRoleNames.Contains(x.Name)).ToList();

            foreach (var role in userRoles)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                var permissions = roleClaims.Where(x => x.Type == Permission.PermissionClaimType &&
                                                        x.Value == requirement.Permission &&
                                                        x.Issuer == "LOCAL AUTHORITY")
                                            .Select(x => x.Value);

                if (permissions.Any())
                {
                    context.Succeed(requirement);
                    return;
                }
            }
        }
    }
}
