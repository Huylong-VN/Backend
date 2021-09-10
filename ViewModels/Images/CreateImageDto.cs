using Microsoft.AspNetCore.Http;

namespace Backend.ViewModels.Images
{
    public class CreateImageDto
    {
        public IFormFile Path { set; get; }
        public string Size { set; get; }
    }
}