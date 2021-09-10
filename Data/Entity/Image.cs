using System;

namespace Backend.Data.Entity
{
    public class Image
    {
        public Guid Id { set; get; }
        public string Path { set; get; }
        public string Size { set; get; }
        public DateTime CreateAt { set; get; }
        public DateTime UpdateAt { set; get; }

        public Guid PostId { set; get; }
        public Post Post { set; get; }
    }
}