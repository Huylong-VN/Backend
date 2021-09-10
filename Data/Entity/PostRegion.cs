using System;

namespace Backend.Data.Entity
{
    public class PostRegion
    {
        public Guid PostId { set; get; }
        public Post Post { set; get; }
        public Guid RegionId { set; get; }
        public Region Region { set; get; }
    }
}