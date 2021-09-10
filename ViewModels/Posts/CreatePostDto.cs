using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Backend.ViewModels.Posts
{
    public class CreatePostDto
    {
        public string Title { set; get; }
        public string Content { set; get; }
        public IFormFile Image { set; get; }
        public Guid CategoryId { set; get; }
        public List<Guid> RegionId { set; get; }
    }
}