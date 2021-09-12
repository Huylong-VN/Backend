using System;

namespace Backend.ViewModels.Contents
{
    public class UpdateContentDto
    {
        public Guid ContentId { set; get; }
        public string ContentItem { set; get; }
    }
}