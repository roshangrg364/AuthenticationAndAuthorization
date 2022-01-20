using InventoryModule.Dto.Category;
using InventoryModule.Entity;
using InventoryModule.Exceptions;
using InventoryModule.Repository;
using InventoryModule.TransactionScopeConfig;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace InventoryModule.Service
{
    public class CategoryService : CategoryServiceInterface
    {
        private readonly CategoryRepositoryInterface _categoryRepo;
        public CategoryService(CategoryRepositoryInterface categoryRepo)
        {

            _categoryRepo = categoryRepo;
        }
        public async Task Activate(int id)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            var category = await _categoryRepo.GetById(id).ConfigureAwait(false) ?? throw new CategoryNotFoundException();

            category.Activate();
            await _categoryRepo.UpdateAsync(category).ConfigureAwait(false);
            tx.Complete();
        }

        public async Task Create(CategoryDto dto)
        {
            using var tx = TransactionScopeHelper.GetInstance();
           await ValidateName(dto.Name).ConfigureAwait(false);
            var category = new Category(dto.Name);
            await _categoryRepo.InsertAsync(category).ConfigureAwait(false);
            tx.Complete();
        }

        public async Task Deactivate(int id)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            var category = await _categoryRepo.GetById(id).ConfigureAwait(false) ?? throw new CategoryNotFoundException();
            category.Deactivate();
            await _categoryRepo.UpdateAsync(category).ConfigureAwait(false);
            tx.Complete();
        }

        public async Task Delete(int id)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            var category = await _categoryRepo.GetById(id).ConfigureAwait(false);
            await _categoryRepo.DeleteAsync(category).ConfigureAwait(false);
            tx.Complete();
        }

        public async Task Update(CategoryDto dto)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            var category = await _categoryRepo.GetById(dto.Id).ConfigureAwait(false) ?? throw new CategoryNotFoundException();
            await ValidateName(dto.Name,category).ConfigureAwait(false);
            category.Udpate(dto.Name);
            await _categoryRepo.UpdateAsync(category).ConfigureAwait(false);
            tx.Complete();
        }

        private async Task ValidateName(string name, Category? category = null)
        {
            var categoryWithSameName = await _categoryRepo.GetByName(name).ConfigureAwait(false);
            if(categoryWithSameName != null && categoryWithSameName !=category)
            {
                throw new DuplicateCategoryException();
            }
        }
    }
}
