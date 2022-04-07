using System;
using System.Threading.Tasks;
using AuthenticationAndAuthorization.Areas.User.Controllers;
using AuthenticationAndAuthorization.Areas.User.ViewModel;
using EmailModule.Entity;
using EmailModule.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using UserModule.Dto.User;
using UserModule.Entity;
using UserModule.Repository;
using UserModule.Service;

namespace AuthenticationAndAuthorization.Controllers
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
        private readonly EmailSenderServiceInterface _emailSenderService;
        public UserRegistrationController(UserRepositoryInterface userRepository,
            IToastNotification notify,
            UserServiceInterface userService,
            ILogger<UserController> logger,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager,
            EmailSenderServiceInterface emailSenderService)
        {
            _userRepo = userRepository;
            _notify = notify;
            _userService = userService;
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
            _emailSenderService = emailSenderService;
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
                var user = await _userManager.FindByEmailAsync(model.EmailAddress).ConfigureAwait(true);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { email = user.Email, token = token }, Request.Scheme);
                var message = new Message(new string[] { "roshan.grg364@gmail.com" }, "test email", "<a href=" + confirmationLink + ">Confirm email</a>", null);
                await _emailSenderService.SendEmail(message);
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
