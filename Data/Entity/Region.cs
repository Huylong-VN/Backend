using System;
using System.Collections.Generic;

namespace Backend.Data.Entity
{
    public class Region
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public DateTime CreateAt { set; get; }
        public DateTime UpdateAt { set; get; }
        public List<PostRegion> PostRegions { set; get; }
    }
}