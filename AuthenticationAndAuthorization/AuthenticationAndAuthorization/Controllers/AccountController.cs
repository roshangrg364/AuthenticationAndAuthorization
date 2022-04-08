using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenticationAndAuthorization.Models;
using EmailModule.Entity;
using EmailModule.Repository;
using EmailModule.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using UserModule.Dto.User;
using UserModule.Entity;
using UserModule.Exceptions;
using UserModule.Repository;
using UserModule.Service;

namespace AuthenticationAndAuthorization.Controllers
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
        private readonly EmailSenderServiceInterface _emailSenderService;
        private readonly EmailTemplateRepositoryInterface _emailTemplateRepo;
        public AccountController(ILogger<AccountController> logger,
            UserRepositoryInterface userRepo,
            IToastNotification notify,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            UserServiceInterface userService,
            EmailSenderServiceInterface emailSender,
            EmailTemplateRepositoryInterface emailTemplateRepo)
        {
            _logger = logger;
            _userRepo = userRepo;
            _notify = notify;
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _emailSenderService = emailSender;
            _emailTemplateRepo = emailTemplateRepo;
        }
        public async Task<IActionResult> Login(string ReturnUrl = "/Home/Index")
        {
            var loginModel = new LoginViewModel()
            {
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
                if (ModelState.IsValid)
                {

                    var user = await _userManager.FindByNameAsync(model.UserName) ?? throw new Exception("Incorrect UserName or Password");

                    if (!user.EmailConfirmed)
                    {
                        throw new Exception("Email not Confirmed");
                    }
                  var isSucceeded =  await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);
                    if (isSucceeded.Succeeded)
                    {
                        _notify.AddSuccessToastMessage("Logged In Successfully");
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return LocalRedirect(model.ReturnUrl);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    if(isSucceeded.IsLockedOut)
                    {
                        return RedirectToAction(nameof(LockOut));
                    }
                  
                }


            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);
                _logger.LogError(ex, ex.Message);

            }
            return RedirectToAction(nameof(Login));
        }

        public IActionResult LockOut()
        {
            return View();
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            _notify.AddSuccessToastMessage("Logged Out Successfully");
            return RedirectToAction(nameof(Login));
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ExternalLogin(string provider, string ReturnUrl)
        {
            try
            {
                var redirectUrl = Url.Action("ExternalLoginCallBack", "Account", new { ReturnUrl = ReturnUrl });
                var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
                return new ChallengeResult(provider, properties);
            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> ExternalLoginCallBack(string ReturnUrl = null, string error = null)
        {
            var loginModel = new LoginViewModel
            {
                ReturnUrl = ReturnUrl,
                ExternalProviders = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            try
            {
                ReturnUrl = ReturnUrl ?? Url.Content("~/");

                if (error != null)
                {
                    ModelState.AddModelError(string.Empty, "error from external login:" + error);
                    return View(nameof(Login), loginModel);
                }
                var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
                if (externalLoginInfo == null)
                {
                    ModelState.AddModelError(string.Empty, "error loading external login information");
                    return View(nameof(Login), loginModel);
                }
                var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
                UserModule.Entity.User user = null;
                if (email != null)
                {
                    user = await _userManager.FindByEmailAsync(email);
                    if (user != null && !user.EmailConfirmed)
                    {
                        throw new Exception("Email Not Confirmed yet");
                    }
                }
                var externalLoginResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, false, true);
                if (externalLoginResult.Succeeded)
                {
                    return LocalRedirect(ReturnUrl);
                }
                else
                {

                    if (email != null)
                    {

                        if (user == null)
                        {
                            var userDto = new UserDto
                            {
                                UserName = email,
                                EmailAddress = email
                            };
                            await _userService.CreateUserForExternalLogin(userDto);
                            user = await _userManager.FindByEmailAsync(email) ?? throw new UserNotFoundException();
                            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            var confirmationLink = Url.Action("ConfirmEmail", "Account", new { email = user.Email, token = token }, Request.Scheme);
                            var confirmationTemplate = await _emailTemplateRepo.GetByType(EmailTemplate.TypeRegistration) ?? throw new Exception("Email Confirmation Template Not Found");
                            var template = confirmationTemplate.Template;
                            template = GenerateTemplate(user, confirmationLink, template);
                            var emailMessage = new Message(new List<string> { user.Email }, "Email Confirmation", template, null);
                            await _emailSenderService.SendEmail(emailMessage);
                            return RedirectToAction("Success","UserRegistration");
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

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(true) ?? throw new UserNotFoundException();
                var isConfirmed = await _userManager.ConfirmEmailAsync(user, token);
                if (isConfirmed.Succeeded)
                {
                    _notify.AddSuccessToastMessage("Email Confirmed Successfully");
                    return RedirectToAction(nameof(Login));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _notify.AddSuccessToastMessage(ex.Message);
            }
            
            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(true) ?? throw new UserNotFoundException();
                if (user != null && user.EmailConfirmed)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(true);
                    var resetPasswordLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);
                    var forgotpasswordTemplate = await _emailTemplateRepo.GetByType(EmailTemplate.TypeForgotPassword) ?? throw new Exception("Forgot password Template Not Defined");
                    var template = forgotpasswordTemplate.Template;
                    template = GenerateTemplate(user, resetPasswordLink, template);
                    var emailMessage = new Message(new List<string> { model.Email }, "forgot password", template, null);
                    await _emailSenderService.SendEmail(emailMessage);
                    return View(nameof(ForgotPasswordConfirmation));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _notify.AddErrorToastMessage(ex.Message);
            }
            return View(model);
        }

        private static string GenerateTemplate(User user, string urlLink, string template)
        {
            var Replacements = new Dictionary<string, string>();
            Replacements.Add("{Name}", user.Name);
            Replacements.Add("{EmailConfirmationLink}", urlLink);
            Replacements.Add("{ResetPasswordLink}", urlLink);
            Replacements.Add("{ForgotPasswordLink}", urlLink);

            foreach (var replacement in Replacements)
            {
                template = template.Replace(replacement.Key, replacement.Value);
            }

            return template;
        }

        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordConfirmation()
        {

            return View();
          
        }
        [AllowAnonymous]
      
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
                {
                    throw new Exception("Invalid email or token");
                }
                var resetPasswordViewModel = new ResetPasswordViewModel() { 
                Token =token,
                Email =email
                };
                return View(resetPasswordViewModel);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                _notify.AddSuccessToastMessage(ex.Message);
            }
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(true) ?? throw new UserNotFoundException();
                   var isResetSuccessfull = await  _userManager.ResetPasswordAsync(user, model.Token, model.Password).ConfigureAwait(true);
                    if(isResetSuccessfull.Succeeded)
                    {
                        if(await _userManager.IsLockedOutAsync(user))
                        {
                         await   _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now);
                        }
                        _notify.AddSuccessToastMessage("Password Reset Successfull");
                        return RedirectToAction(nameof(Login));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _notify.AddSuccessToastMessage(ex.Message);
            }
            return RedirectToAction(nameof(Login));
        }
    }
}
