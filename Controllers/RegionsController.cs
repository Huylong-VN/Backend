using Backend.Application.Interfaces;
using Backend.ViewModels.Common;
using Backend.ViewModels.Regions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionService _regionService;

        public RegionsController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync([FromQuery] PagedAndSortedResultRequestDto request)
        {
            return Ok(await _regionService.GetListAsync(request));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateRegionDto request)
        {
            var kq = await _regionService.CreateAsync(request);
            if (kq == true) return Ok(kq);
            return BadRequest(kq);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateAsync(Guid Id, [FromBody] UpdateRegionDto request)
        {
            var kq = await _regionService.UpdateAsync(Id, request);
            if (kq == true) return Ok(kq);
            return BadRequest(kq);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAsync(Guid Id)
        {
            var kq = await _regionService.DeleteAsync(Id);
            if (kq == true) return Ok(kq);
            return BadRequest(kq);
        }
    }
}