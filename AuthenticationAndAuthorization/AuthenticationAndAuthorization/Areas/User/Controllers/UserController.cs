using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthenticationAndAuthorization.Areas.User.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NToastNotify;
using UserModule.Dto.User;
using UserModule.Exceptions;
using UserModule.Repository;
using UserModule.Service;
using UserEntity = UserModule.Entity.User;
namespace AuthenticationAndAuthorization.Areas.User.Controllers
{
    [Area("User")]
    
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
            RoleManager<IdentityRole> roleManager,
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
       [Authorize(Policy ="User.View")]
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
                    Status = user.Status,
                    Type = user.Type
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
                    UserName = model.UserName,
                    Type = UserModule.Entity.User.TypeGeneral
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
                foreach (var role in roles)
                {
                    var roleModel = new RolesViewModel()
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,
                        IsSelected = false
                    };
                    if (await _userManager.IsInRoleAsync(user, role.Name))
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
                var assignRoleDto = new AssignRoleDto
                {
                    UserId = model.UserId,
                };
                foreach (var roleModel in model.Roles)
                {
                    assignRoleDto.Roles.Add(new RolesDto
                    {
                        Role = roleModel.RoleId,
                        IsSelected = roleModel.IsSelected
                    });
                }
                await _userService.AssignRole(assignRoleDto);
                _notify.AddSuccessToastMessage("Roles Added Successfully");

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
