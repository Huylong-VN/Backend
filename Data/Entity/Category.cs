using System;
using System.Collections.Generic;

namespace Backend.Data.Entity
{
    public class Category
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public DateTime CreateAt { set; get; }
        public DateTime UpdateAt { set; get; }
        public List<Post> Posts { set; get; }
    }
}