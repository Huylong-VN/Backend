using System;
using System.Collections.Generic;

namespace Backend.Data.Entity
{
    public class Content
    {
        public Guid Id { get; set; }
        public string ContentItem { get; set; }
        public List<Image> Images { get; set; }
        public Post Post { get; set; }
        public Guid PostId { set; get; }
        public DateTime CreateAt { set; get; }
        public DateTime UpdateAt { set; get; }
    }
}