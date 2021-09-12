using System;

namespace Backend.ViewModels.Posts
{
    public class UpdatePostDto
    {
        public string Title { set; get; }
        public Guid CategoryId { set; get; }
    }
}