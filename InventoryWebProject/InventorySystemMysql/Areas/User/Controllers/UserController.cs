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
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.Extensions.Logging;

namespace InventorySystemMysql.Areas.User.Controllers
{
    [Area("User")]
    public class UserController : Controller
    {
        private readonly UserRepositoryInterface _userRepo;
        private readonly IToastNotification _notify;
        private readonly UserServiceInterface _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(UserRepositoryInterface userRepository,
            IToastNotification notify,
            UserServiceInterface userService)
        {
            _userRepo = userRepository;
            _notify = notify;
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var users = await _userRepo.GetAllAsync();
                var userIndexViewModels = new List<UserIndexViewModel>();
                foreach (var user in users)
                {
                    userIndexViewModels.Add(new UserIndexViewModel
                    {
                        //Id = user.Id,
                        //Name = user.Name,
                        //EmailAddress  = user.EmailAddress,
                        //MobileNumber  =user.MobileNumber,
                        //UserName = user.UserName,
                        Status = user.Status
                    });
                }
                return Json(await userIndexViewModels.ToDataSourceResultAsync(request));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }

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
    }
}
