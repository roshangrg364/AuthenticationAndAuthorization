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
using UserModule.Entity;
using UserModule.Exceptions;
using UserModule.Repository;
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
        public AccountController(ILogger<AccountController> logger,
            UserRepositoryInterface userRepo,
            IToastNotification notify,
            UserManager<User> userManager,
            SignInManager<User> signInManager) 
        {
            _logger = logger;
            _userRepo = userRepo;
            _notify = notify;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async  Task<IActionResult> Login(string ReturnUrl = "/Home/Index")
        {
            var loginModel = new LoginViewModel() { 
            ReturnUrl = ReturnUrl};

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
    }
}
