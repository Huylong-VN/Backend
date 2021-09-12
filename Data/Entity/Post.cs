using System;
using System.Collections.Generic;

namespace Backend.Data.Entity
{
    public class Post
    {
        public Guid Id { set; get; }
        public string Title { set; get; }
        public DateTime CreateAt { set; get; }
        public DateTime UpdateAt { set; get; }
        public Guid CategoryId { set; get; }
        public Category Category { set; get; }
        public List<PostRegion> PostRegions { set; get; }
        public List<Content> Contents { set; get; }
    }
}