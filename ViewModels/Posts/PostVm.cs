using Backend.Data.Entity;
using Backend.ViewModels.Categories;
using Backend.ViewModels.Contents;
using Backend.ViewModels.Regions;
using System;
using System.Collections.Generic;

namespace Backend.ViewModels.Posts
{
    public class PostVm
    {
        public Guid Id { set; get; }
        public string Title { set; get; }
        public List<ContentVm> Contents { set; get; }
        public CategoryVm Category { set; get; }
        public List<RegionVm> Region { set; get; }
        public DateTime CreateAt { set; get; }
        public DateTime UpdateAt { set; get; }
    }
}