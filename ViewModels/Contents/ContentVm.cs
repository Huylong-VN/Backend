using Backend.ViewModels.Images;
using System;
using System.Collections.Generic;

namespace Backend.ViewModels.Contents
{
    public class ContentVm
    {
        public Guid Id { get; set; }
        public string ContentItem { get; set; }
        public DateTime CreateAt { set; get; }
        public DateTime UpdateAt { set; get; }
        public List<ImageVm> Images { set; get; }
    }
}