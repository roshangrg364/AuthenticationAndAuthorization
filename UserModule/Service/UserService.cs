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
        public UserService(UserRepositoryInterface userRepo, UserManager<User> userManager)
        {
            _userRepo = userRepo;
            _userManager = userManager;
        }
        public async Task Activate(long id)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            var user = await _userRepo.GetById(id).ConfigureAwait(false) ?? throw new UserNotFoundException();
            user.Activate();
            await _userRepo.UpdateAsync(user).ConfigureAwait(false);
            tx.Complete();
        }

        public async Task Create(UserDto dto)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            await ValidateUser(dto.MobileNumber, dto.EmailAddress);
            var user = new User(dto.Name, dto.UserName, dto.Password,dto.EmailAddress, dto.MobileNumber);
            // var hashPassword = crypter.HashPassword(dto.Password);
            //  user.SetPassword(hashPassword);
            await _userManager.CreateAsync(user);
            //await _userRepo.InsertAsync(user).ConfigureAwait(false);
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
            var userWithSameMobile = await _userRepo.GetByMobile(mobile).ConfigureAwait(false);
            if (userWithSameMobile != null && user != userWithSameMobile) throw new Exception("User With Same Number Already Exists");
                var userWithSameEmail = await _userRepo.GetByEmail(email).ConfigureAwait(false);
                if (userWithSameEmail != null && user != userWithSameEmail) throw new Exception("User With Same Email Already Exists");
         
        }
    }
}
