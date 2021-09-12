using System;

namespace Backend.Data.Entity
{
    public class Image
    {
        public Guid Id { set; get; }
        public string Path { set; get; }
        public long Size { set; get; }
        public DateTime CreateAt { set; get; }
        public DateTime UpdateAt { set; get; }
        public bool isDefault { set; get; }

        public Guid ContentId { set; get; }
        public Content Content { set; get; }
    }
}