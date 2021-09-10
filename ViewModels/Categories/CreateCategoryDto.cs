using System;

namespace Backend.ViewModels.Categories
{
    public class CreateCategoryDto
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public DateTime CreateAt { set; get; }
    }
}