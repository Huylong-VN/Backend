using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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