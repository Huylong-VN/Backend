using System;

namespace Backend.ViewModels.Images
{
    public class ImageVm
    {
        public Guid Id { set; get; }
        public string Path { set; get; }
        public string Size { set; get; }
        public DateTime CreateAt { set; get; }
        public DateTime UpdateAt { set; get; }
    }
}