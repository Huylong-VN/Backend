using System;

namespace Backend.ViewModels.Regions
{
    public class RegionVm
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public DateTime CreateAt { set; get; }
        public DateTime UpdateAt { set; get; }
    }
}