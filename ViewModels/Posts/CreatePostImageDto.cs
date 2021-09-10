using Microsoft.AspNetCore.Http;
using System;

namespace Backend.ViewModels.Posts
{
    public class CreatePostImageDto
    {
        public Guid postId { set; get; }
        public IFormFile image { set; get; }
    }
}