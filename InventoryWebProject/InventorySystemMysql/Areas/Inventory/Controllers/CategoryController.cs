using InventoryModule.Dto.Category;
using InventoryModule.Exceptions;
using InventoryModule.Repository;
using InventoryModule.Service;
using InventorySystemMysql.ActionFilters;
using InventorySystemMysql.Areas.Inventory.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystemMysql.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Authorize]
    
    public class CategoryController : Controller
    {
        private readonly CategoryRepositoryInterface _categoryRepo;
        private readonly ILogger<CategoryController> _logger;
        private readonly IToastNotification _notify;
        private readonly CategoryServiceInterface _categoryService;
        public CategoryController(CategoryRepositoryInterface categoryRepo,
            IToastNotification notify,
            CategoryServiceInterface categoryService,
            ILogger<CategoryController> logger)
        {
            _categoryRepo = categoryRepo;
            _logger = logger;
            _notify = notify;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepo.GetAllAsync();
            var categoryIndexModels = new List<CategoryIndexViewModel>();
            foreach (var category in categories)
            {
                categoryIndexModels.Add(new CategoryIndexViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Status = category.Status
                });
            }
            return View(categoryIndexModels);
        }

   
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var createDto = new CategoryDto() { Name = model.Name };
                    await _categoryService.Create(createDto);
                    _notify.AddSuccessToastMessage("CreatedSuccessfully");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(long categoryId)
        {
            try
            {
                var category = await _categoryRepo.GetById(categoryId) ?? throw new CategoryNotFoundException();
                var categoryUpdateModel = new CategoryViewModel() {
                    Id  = categoryId,
                    Name = category.Name
                };
                return View(categoryUpdateModel);
            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryIndexViewModel model)
        {
            try
            {
                var categoryUpdateDto = new CategoryDto { 
                    Id = model.Id,
                    Name = model.Name
                };
                await _categoryService.Update(categoryUpdateDto);
                _notify.AddSuccessToastMessage("CreatedSuccessfully");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);
                _logger.LogError(ex, ex.Message);
            }
            return View(model);
        }
    }
}
