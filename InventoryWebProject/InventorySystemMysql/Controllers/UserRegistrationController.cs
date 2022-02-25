using InventorySystemMysql.Areas.User.Controllers;
using InventorySystemMysql.Areas.User.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserModule.Dto.User;
using UserModule.Entity;
using UserModule.Repository;
using UserModule.Service;

namespace InventorySystemMysql.Controllers
{
    [AllowAnonymous]
    public class UserRegistrationController : Controller
    {
        private readonly UserRepositoryInterface _userRepo;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IToastNotification _notify;
        private readonly UserServiceInterface _userService;
        private readonly ILogger<UserController> _logger;
        public UserRegistrationController(UserRepositoryInterface userRepository,
            IToastNotification notify,
            UserServiceInterface userService,
            ILogger<UserController> logger,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _userRepo = userRepository;
            _notify = notify;
            _userService = userService;
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            try
            {
                var createDto = new UserDto()
                {
                    Name = model.Name,
                    EmailAddress = model.EmailAddress,
                    MobileNumber = model.MobileNumber,
                    Password = model.Password,
                    UserName = model.UserName,
                    Type = UserModule.Entity.User.TypeCustomer
                };
                await _userService.Create(createDto);
                _notify.AddSuccessToastMessage("created succesfuly");
                var user = await _userManager.FindByNameAsync(model.Name).ConfigureAwait(true);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);
                _logger.Log(LogLevel.Warning, confirmationLink);
                return RedirectToAction(nameof(Success));
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _notify.AddErrorToastMessage(ex.Message);
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
