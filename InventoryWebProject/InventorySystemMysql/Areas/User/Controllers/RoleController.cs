using InventorySystemMysql.Areas.User.ViewModel;
using InventorySystemMysql.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserModule.Dto.Role;
using UserModule.Exceptions;
using UserModule.PermissionHandler;
using UserModule.Repository;
using UserModule.Service;

namespace InventorySystemMysql.Areas.User.Controllers
{
    [Area("User")]
    
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserModule.Entity.User> _userManager;
        private readonly RoleServiceInterface _roleService;
        private readonly ILogger<RoleController> _logger;
        private readonly IToastNotification _notify;
        public RoleController(RoleManager<IdentityRole> roleManager,
          RoleServiceInterface roleService,
          ILogger<RoleController> logger,
          IToastNotification notify,
          UserManager<UserModule.Entity.User> userManager)
        {
            _roleManager = roleManager;
            _roleService = roleService;
            _logger = logger;
            _notify = notify;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            var roleIndexViewModels = new List<RoleIndexViewModel>();
            var i = 1;
            foreach (var role in roles)
            {
                roleIndexViewModels.Add(new RoleIndexViewModel
                {
                    Sno = i,
                    Id = role.Id,
                    Name = role.Name,

                });
                i++;
              
            }
            return View(roleIndexViewModels);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            try
            {
                var dto = new RoleDto() { RoleName = model.RoleName };
                await _roleService.Create(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
            return View(model);
        }

        public async Task<IActionResult> AssignUser(string roleId)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId) ?? throw new Exception("Role Not Found");
                var users =  await _userManager.Users.ToListAsync();
                var assignUserIndexViewModel = new AssignRoleToUserIndexViewModel();
                assignUserIndexViewModel.RoleId = roleId;
                assignUserIndexViewModel.RoleName = role.Name;
                foreach(var user in users)
                {
                    var userModel = new AssignRoleToUserViewModel
                    {
                        UserId = user.Id,
                        Name = user.UserName
                    };
                  
                    if( await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        userModel.IsSelected = true;
                    }
                    else
                    {
                        userModel.IsSelected = false;
                    }
                    assignUserIndexViewModel.Users.Add(userModel);
                }
                return View(assignUserIndexViewModel);
            }
            catch (Exception ex)
            {

                _notify.AddErrorToastMessage(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AssignUser(AssignRoleToUserIndexViewModel model)
        {
            try
            {
                var assignUserToRoleDto = new AssignUsersToRoleDto()
                {
                    RoleId = model.RoleId
                };
                foreach(var user in model.Users)
                {
                    assignUserToRoleDto.Users.Add(new AssignUserDto
                    {
                        UserId = user.UserId,
                        IsSelected = user.IsSelected
                    });
                }
              await   _roleService.AssingUser(assignUserToRoleDto);
                _notify.AddSuccessToastMessage("Users Added");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                _notify.AddErrorToastMessage(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(string roleId)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId) ?? throw new RoleNotFoundException();
                var roleEditViewModel = new RoleEditViewModel() { 
                Id = roleId,
                Name = role.Name
                };
                return View(roleEditViewModel);
            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);
                _logger.LogError(ex, ex.Message);            
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(RoleEditViewModel model)
        {
            try
            {
                
            }
            catch (Exception ex)
            {

                _notify.AddErrorToastMessage(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AssignPermission(string roleId)
        {
            try
            {

                var role = await _roleManager.FindByIdAsync(roleId) ?? throw new RoleNotFoundException();
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                var permissionIndexModel = new RolePermissionViewModel() { 
                RoleId = roleId,
                RoleName = role.Name
                };

                foreach(var claim in Permission.Permissions)
                {
                    var claimModel = new AssignPermissionViewModel {
                    Permission = claim
                    };
                    if(roleClaims.Any(a=>a.Value == claim))
                    {
                        claimModel.IsSelected = true;
                    }
                    permissionIndexModel.Permissions.Add(claimModel);
                }
                return View(permissionIndexModel);
            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> AssignPermission(RolePermissionViewModel model)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(model.RoleId) ?? throw new RoleNotFoundException();
                var claims = await _roleManager.GetClaimsAsync(role);
                foreach(var claim in claims)
                {
                    var result = await _roleManager.RemoveClaimAsync(role, claim);
                    if (!result.Succeeded) throw new Exception("Error in setting permission");
                }
                foreach(var permission in model.Permissions)
                {
                    if(permission.IsSelected == true)
                    {
                        await _roleManager.AddClaimAsync(role, new Claim(Permission.PermissionClaimType,permission.Permission));
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
