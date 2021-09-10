using Backend.Application.Interfaces;
using Backend.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetListAsync()
        {
            return Ok(await _categoryService.GetListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateCategoryDto request)
        {
            var result = await _categoryService.CreateAsync(request);
            if (result == true) return Ok(result);
            return BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> CreateAsync(Guid Id, UpdateCategoryDto request)
        {
            var result = await _categoryService.UpdateAsync(Id, request);
            if (result == true) return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> CreateAsync(Guid Id)
        {
            var result = await _categoryService.DeleteAsync(Id);
            if (result == true) return Ok(result);
            return BadRequest(result);
        }
    }
}