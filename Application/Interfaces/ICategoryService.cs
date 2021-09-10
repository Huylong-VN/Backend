using Backend.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryVm>> GetListAsync();

        Task<bool> CreateAsync(CreateCategoryDto request);

        Task<bool> UpdateAsync(Guid Id, UpdateCategoryDto request);

        Task<bool> DeleteAsync(Guid Id);
    }
}