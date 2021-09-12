using Backend.Application.Interfaces;
using Backend.ViewModels.Contents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentsController : BaseController
    {
        private readonly IContentService _contentService;

        public ContentsController(IContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var result = await _contentService.GetByIdAsync(Id);
            if (result.IsSuccessed) return Ok(result.ResultObj);
            return BadRequest(result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateContentDto request)
        {
            var result = await _contentService.UpdateContentAsync(request);
            if (result == true) return Ok(true);
            return BadRequest(false);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _contentService.RemoveImagePostContent(Id);
            if (result == true) return Ok(true);
            return BadRequest(false);
        }
    }
}