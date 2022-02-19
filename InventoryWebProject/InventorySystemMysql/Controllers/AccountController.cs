using InventorySystemMysql.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserModule.Dto.User;
using UserModule.Entity;
using UserModule.Exceptions;
using UserModule.Repository;
using UserModule.Service;
using crypter = BCrypt.Net.BCrypt;
namespace InventorySystemMysql.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserRepositoryInterface _userRepo;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IToastNotification _notify;
        private readonly UserServiceInterface _userService;
        public AccountController(ILogger<AccountController> logger,
            UserRepositoryInterface userRepo,
            IToastNotification notify,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            UserServiceInterface userService) 
        {
            _logger = logger;
            _userRepo = userRepo;
            _notify = notify;
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
        }
        public async  Task<IActionResult> Login(string ReturnUrl = "/Home/Index")
        {
            var loginModel = new LoginViewModel() {
                ReturnUrl = ReturnUrl,
                ExternalProviders = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
            if(ModelState.IsValid)
                {
                    
                        var user = await _userManager.FindByNameAsync(model.UserName) ?? throw new Exception("Incorrect UserName or Password");
                        var IsPasswordCorrect = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                        if (!IsPasswordCorrect.Succeeded) throw new Exception("Incorrect UserName or Password");
                    _notify.AddSuccessToastMessage("Logged In Successfully");
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return LocalRedirect(model.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                    }
               

            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);
                _logger.LogError(ex, ex.Message);
               
            }
            return RedirectToAction(nameof(Login));
        }


        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            _notify.AddSuccessToastMessage("Logged Out Successfully");
            return RedirectToAction(nameof(Login));
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ExternalLogin(string provider,string ReturnUrl)
        {
            try
            {
                var redirectUrl = Url.Action("ExternalLoginCallBack", "Account", new { ReturnUrl = ReturnUrl });
                var properties =  _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
                return new ChallengeResult(provider, properties);
            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> ExternalLoginCallBack(string ReturnUrl =null,string error = null)
        {
            var loginModel = new LoginViewModel
            {
                ReturnUrl = ReturnUrl,
                ExternalProviders = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            try
            {
                ReturnUrl = ReturnUrl ?? Url.Content("~/");
               
                if(error !=null)
                {
                    ModelState.AddModelError(string.Empty, "error from external login:" + error);
                    return View(nameof(Login),loginModel);
                }
                var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
                if (externalLoginInfo == null)
                {
                    ModelState.AddModelError(string.Empty, "error loading external login information");
                    return View(nameof(Login),loginModel);
                }

               var externalLoginResult= await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, false, true);
                if (externalLoginResult.Succeeded)
                {
                    return LocalRedirect(ReturnUrl);
                }
                else
                {
                    var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
                    if (email != null)
                    {

                        var user = await _userManager.FindByEmailAsync(email);
                        if (user == null)
                        {
                            var userDto = new UserDto
                            {
                                UserName = email,
                                EmailAddress = email
                            };
                            await _userService.CreateUserForExternalLogin(userDto);
                            user = await _userManager.FindByEmailAsync(email) ?? throw new UserNotFoundException();
                        }
                        await _userManager.AddLoginAsync(user, externalLoginInfo);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(ReturnUrl);
                    }


                }
            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
          
            return RedirectToAction(nameof(Login), loginModel);
        }
    }
}
