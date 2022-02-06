using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserModule.Dto.Role;
using UserModule.Entity;
using UserModule.Exceptions;
using UserModule.TransactionScopeConfig;

namespace UserModule.Service
{
    public class RoleService : RoleServiceInterface
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        public RoleService(RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task AssingUser(AssignUsersToRoleDto dto)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            var role = await _roleManager.FindByIdAsync(dto.RoleId) ?? throw new Exception("Role Not Found");
            foreach (var user in dto.Users)
            {
                var User = await _userManager.FindByIdAsync(user.UserId) ?? throw new UserNotFoundException();
                if (!await _userManager.IsInRoleAsync(User, role.Name) && user.IsSelected)
                {
                    await _userManager.AddToRoleAsync(User, role.Name);
                }
                if (await _userManager.IsInRoleAsync(User, role.Name) && !user.IsSelected)
                {
                    await _userManager.RemoveFromRoleAsync(User, role.Name);
                }
            }
            tx.Complete();
        }

        public async Task Create(RoleDto dto)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            var identityRole = new IdentityRole()
            {
                Name = dto.RoleName
            };
            await _roleManager.CreateAsync(identityRole).ConfigureAwait(false);
            tx.Complete();
        }
    }
}
