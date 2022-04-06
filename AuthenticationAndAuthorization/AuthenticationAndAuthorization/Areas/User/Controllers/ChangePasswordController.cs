using InventorySystemMysql.Areas.User.ViewModel;
using InventorySystemMysql.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserModule.Exceptions;

namespace InventorySystemMysql.Areas.User.Controllers
{
    [Area("User")]
    public class ChangePasswordController : Controller
    {
        private readonly UserManager<UserModule.Entity.User> _userManager;
        private readonly ILogger<ChangePasswordController> _logger;
        private readonly IToastNotification _notify;
        public ChangePasswordController(UserManager<UserModule.Entity.User> userManager,
            ILogger<ChangePasswordController> logger,
            IToastNotification notify)
        {
            _logger = logger;
            _userManager = userManager;
            _notify = notify;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(ChangePasswordViewModel model)
        {
            try
            {
                var userId = this.GetCurrentUserId();
                var user = await _userManager.FindByIdAsync(userId) ?? throw new UserNotFoundException();
                if (ModelState.IsValid)
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.OldPasword, model.NewPassword).ConfigureAwait(true);
                    if(result.Succeeded)
                    {
                        _notify.AddSuccessToastMessage("Password Changed Successfully");
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("error", error.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _notify.AddErrorToastMessage(ex.Message);
            }
            return View(model);
        }

        public async Task<IActionResult> AddPassword()
        {
            try
            {
                var user = await this.GetCurrentUser();
                var hasPassword = await _userManager.HasPasswordAsync(user);
                if (hasPassword) return RedirectToAction(nameof(Index));
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _notify.AddErrorToastMessage(ex.Message);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpPost]
        public async Task<IActionResult> AddPassword(AddPasswordViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var userId =  this.GetCurrentUserId();
                    var user = await _userManager.FindByIdAsync(userId) ?? throw new UserNotFoundException();
                    var isSucceded = await _userManager.AddPasswordAsync(user, model.ConfirmPassword);
                    if (isSucceded.Succeeded)
                    {
                        _notify.AddSuccessToastMessage("Password added Successfully");
                        return RedirectToAction("Index", "Home",new { area = "" });
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _notify.AddErrorToastMessage(ex.Message);
            }
            return View(model);
        }
    }
}
