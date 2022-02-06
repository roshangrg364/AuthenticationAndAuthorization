using InventorySystemMysql.Areas.User.ViewModel;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserModule.Dto.User;
using UserModule.Repository;
using UserModule.Service;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserModule.Exceptions;
using UserEntity = UserModule.Entity.User;
namespace InventorySystemMysql.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly UserRepositoryInterface _userRepo;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IToastNotification _notify;
        private readonly UserServiceInterface _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(UserRepositoryInterface userRepository,
            IToastNotification notify,
            UserServiceInterface userService,
            ILogger<UserController> logger,
            RoleManager<IdentityRole>  roleManager,
            UserManager<UserEntity> userManager
            )
        {
            _userRepo = userRepository;
            _notify = notify;
            _userService = userService;
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userRepo.GetAllAsync();
            var userIndexViewModels = new List<UserIndexViewModel>();
            var i = 1;
            foreach (var user in users)
            {
                userIndexViewModels.Add(new UserIndexViewModel
                {  
                    SNo = i,
                    Id = user.Id,
                    Name = user.Name,
                    EmailAddress = user.Email,
                    MobileNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Status = user.Status
                });
                i++;
            }
            return View(userIndexViewModels);
        }

       public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            try
            {
                var createDto = new UserDto()
                {
                    Name = model.Name,
                    EmailAddress = model.EmailAddress,
                    MobileNumber = model.MobileNumber,
                    Password = model.Password,
                    UserName = model.UserName
                };
                await _userService.Create(createDto);
                _notify.AddSuccessToastMessage("created succesfuly");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);
               
            }
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> AssignRole(string userId)
        {
            try
            {
                var user = await _userRepo.GetByIdString(userId) ?? throw new UserNotFoundException();
                var roles = await _roleManager.Roles.ToListAsync();
                var assignRoleViewModel = new AssignRoleViewModel
                {
                    UserId = userId,
                    UserName = user.Name
                };
                foreach(var role in roles)
                {
                    var roleModel = new RolesViewModel() { 
                    RoleId = role.Id,
                    RoleName= role.Name,
                    IsSelected= false
                    };
                    if(await _userManager.IsInRoleAsync(user,role.Name) )
                    {
                        roleModel.IsSelected = true;
                    }
                    assignRoleViewModel.Roles.Add(roleModel);
                }

                return View(assignRoleViewModel);
            }
            catch (Exception ex)
            {

                _notify.AddErrorToastMessage(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(AssignRoleViewModel model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId).ConfigureAwait(false) ?? throw new UserNotFoundException();
                foreach(var role in model.Roles)
                {
                    var userRole = await _roleManager.FindByIdAsync(role.RoleId).ConfigureAwait(false) ?? throw new RoleNotFoundException();
                    if(!await _userManager.IsInRoleAsync(user,userRole.Name) && role.IsSelected == true)
                    {
                        await _userManager.AddToRoleAsync(user, userRole.Name);
                    }
                    if(await _userManager.IsInRoleAsync(user, userRole.Name) && role.IsSelected == false)
                    {
                        await _userManager.RemoveFromRoleAsync(user, userRole.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
