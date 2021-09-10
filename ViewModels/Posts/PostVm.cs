using Backend.Data.Entity;
using Backend.ViewModels.Categories;
using Backend.ViewModels.Images;
using Backend.ViewModels.Regions;
using System;
using System.Collections.Generic;

namespace Backend.ViewModels.Posts
{
    public class PostVm
    {
        public Guid Id { set; get; }
        public string Title { set; get; }
        public string Content { set; get; }
        public List<ImageVm> Images { set; get; }
        public CategoryVm Category { set; get; }
        public List<RegionVm> Region { set; get; }
        public DateTime CreateAt { set; get; }
        public DateTime UpdateAt { set; get; }
    }
}