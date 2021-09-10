using System;

namespace Backend.ViewModels.Common
{
    public class ListResultDto
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public bool Selected { set; get; }
    }
}