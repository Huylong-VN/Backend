using AutoMapper;
using Backend.Application.Interfaces;
using Backend.Data.DbContext;
using Backend.Data.Entity;
using Backend.ViewModels.Categories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Application.Implements
{
    public class CategoryService : ICategoryService
    {
        private readonly NewsAppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(NewsAppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> CreateAsync(CreateCategoryDto request)
        {
            var category = new Category()
            {
                CreateAt = DateTime.Now,
                Name = request.Name
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            var category = await _context.Categories.FindAsync(Id);
            if (category == null) return false;
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CategoryVm>> GetListAsync()
        {
            var category = await _context.Categories.ToListAsync();
            return _mapper.Map<List<CategoryVm>>(category);
        }

        public async Task<bool> UpdateAsync(Guid Id, UpdateCategoryDto request)
        {
            var category = await _context.Categories.FindAsync(Id);
            if (category == null) return false;
            category.UpdateAt = DateTime.Now;
            category.Name = request.Name;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}