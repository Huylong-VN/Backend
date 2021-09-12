using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Backend.ViewModels.Posts
{
    public class CreateContentDto
    {
        public Guid postId { set; get; }
        public string content { set; get; }
        public List<IFormFile> image { set; get; }
    }
}