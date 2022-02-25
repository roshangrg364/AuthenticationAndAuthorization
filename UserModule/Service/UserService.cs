using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserModule.Dto.User;
using UserModule.Entity;
using UserModule.Exceptions;
using UserModule.Repository;
using UserModule.TransactionScopeConfig;
using crypter =BCrypt.Net.BCrypt;
namespace UserModule.Service
{
    public class UserService : UserServiceInterface
    {
        private readonly UserRepositoryInterface _userRepo;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserService(UserRepositoryInterface userRepo,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userRepo = userRepo;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task Activate(long id)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            var user = await _userRepo.GetById(id).ConfigureAwait(false) ?? throw new UserNotFoundException();
            user.Activate();
            await _userRepo.UpdateAsync(user).ConfigureAwait(false);
            tx.Complete();
        }

        public async Task AssignRole(AssignRoleDto dto)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            var user = await _userManager.FindByIdAsync(dto.UserId).ConfigureAwait(false) ?? throw new UserNotFoundException();
            foreach (var role in dto.Roles)
            {
                var userRole = await _roleManager.FindByIdAsync(role.Role).ConfigureAwait(false) ?? throw new RoleNotFoundException();
                if (!await _userManager.IsInRoleAsync(user, userRole.Name) && role.IsSelected == true)
                {
                    await _userManager.AddToRoleAsync(user, userRole.Name);
                }
                if (await _userManager.IsInRoleAsync(user, userRole.Name) && role.IsSelected == false)
                {
                    await _userManager.RemoveFromRoleAsync(user, userRole.Name);
                }
            }
            tx.Complete();
        }

        public async Task Create(UserDto dto)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            await ValidateUser(dto.MobileNumber, dto.EmailAddress);
            var user = new User(dto.UserName, dto.EmailAddress,dto.Type) { 
            PhoneNumber = dto.MobileNumber,
            Name = dto.Name
            };
            var result =await _userManager.CreateAsync(user,dto.Password);
            if (!result.Succeeded) {
                var errors = "";
                foreach (var error in result.Errors)
                {
                   errors = errors +"_" + error.Description;
                }
                throw new Exception(errors);
            }
          
            tx.Complete();
        }

        public async Task CreateUserForExternalLogin(UserDto dto)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            await ValidateUser(dto.MobileNumber, dto.EmailAddress);
            var user = new User(dto.UserName, dto.EmailAddress,User.TypeExternal)
            {
                PhoneNumber = dto.MobileNumber,
                Name = dto.Name
            };
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                var errors = "";
                foreach (var error in result.Errors)
                {
                    errors = errors + "_" + error.Description;
                }
                throw new Exception(errors);
            }

            tx.Complete();
        }

        public async Task Deactivate(long id)
        {
            throw new NotImplementedException();
        }

        public async Task Update(UserDto dto)
        {
            throw new NotImplementedException();
        }

        private async Task ValidateUser(string mobile,string email, User? user = null)
        {
            if (string.IsNullOrWhiteSpace(mobile) || string.IsNullOrWhiteSpace(email)) return;
            var userWithSameMobile = await _userRepo.GetByMobile(mobile).ConfigureAwait(false);
            if (userWithSameMobile != null && user != userWithSameMobile) throw new Exception("User With Same Number Already Exists");
                var userWithSameEmail = await _userRepo.GetByEmail(email).ConfigureAwait(false);
                if (userWithSameEmail != null && user != userWithSameEmail) throw new Exception("User With Same Email Already Exists");
         
        }
    }
}
