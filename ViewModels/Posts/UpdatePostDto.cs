using System;

namespace Backend.ViewModels.Posts
{
    public class UpdatePostDto
    {
        public string Title { set; get; }
        public string Content { set; get; }
        public Guid CategoryId { set; get; }
        public Guid RegionId { set; get; }
    }
}