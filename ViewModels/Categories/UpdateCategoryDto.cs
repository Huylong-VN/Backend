using System;

namespace Backend.ViewModels.Categories
{
    public class UpdateCategoryDto
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public DateTime UpdateAt { set; get; }
    }
}