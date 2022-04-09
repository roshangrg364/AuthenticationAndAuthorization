using AuthenticationAndAuthorization.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserModule.Authentication;
using UserModule.Dto;
using UserModule.Entity;

namespace AuthenticationAndAuthorization.Controllers.Api
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class LoginApiController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenGeneratorInterface _tokenGenerator;
        private readonly ILogger<LoginApiController> _logger;
        private readonly IConfiguration _configuration;
        public LoginApiController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            TokenGeneratorInterface tokenGenerator,
            ILogger<LoginApiController> logger,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _logger = logger;
            _configuration = configuration;
            _signInManager = signInManager;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()

        {
            var user = await _userManager.Users.ToListAsync();
          var firstUser = user.First();

            var model = new
            {
                name = firstUser.Name
            };
            return Ok(model);
                }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
               
                    var user = await _userManager.FindByNameAsync(model.UserName) ?? throw new Exception("Incorrect UserName or Password");

                    if (!user.EmailConfirmed)
                    {
                        throw new Exception("Email not Confirmed");
                    }
                    var isSucceeded = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);
                    if (isSucceeded.Succeeded)
                    {
                        var tokenDto = new TokenDto() { 
                        UserId = user.Id,
                        UserName= user.UserName,
                        Email = user.Email,
                        JwtKey = _configuration["JWT:Secret"]
                        };
                        var token = _tokenGenerator.GenerateToken(tokenDto);
                    return Ok(token);
                    }
                    return BadRequest("Incorrect user name or password");
                
            }
            catch (Exception ex)
            {
                return  BadRequest(ex.Message);
            }
           

        }
    }
}
