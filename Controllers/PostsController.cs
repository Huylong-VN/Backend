using Backend.Application.Interfaces;
using Backend.SignalR;
using Backend.ViewModels.Common;
using Backend.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    public class PostsController : BaseController
    {
        private readonly IPostService _postService;
        private readonly IHubContext<SignalHub> _hubContext;

        public PostsController(IPostService postService, IHubContext<SignalHub> hubContext)
        {
            _hubContext = hubContext;
            _postService = postService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetListAsync([FromQuery] PagedAndSortedResultRequestDto request)
        {
            return Ok(await _postService.GetListAsync(request));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreatePostDto request)
        {
            var result = await _postService.CreateAsync(request);
            if (result != null)
            {
                await _hubContext.Clients.All.SendAsync("GetPosts");
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateAsync(Guid Id, [FromBody] UpdatePostDto request)
        {
            var result = await _postService.UpdateAsync(Id, request);
            if (result == null) return BadRequest(result.Message);
            await _hubContext.Clients.All.SendAsync("GetPosts");
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAsync(Guid Id)
        {
            var result = await _postService.DeleteAsync(Id);
            if (result == true)
            {
                await _hubContext.Clients.All.SendAsync("GetPosts");
                return Ok(result);
            }
            return BadRequest(false);
        }

        [HttpGet("Category")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListCategoryAsync()
        {
            await _hubContext.Clients.All.SendAsync("GetPosts");
            return Ok(await _postService.GetListCategory());
        }

        [HttpDelete("postregion")]
        public async Task<IActionResult> DeletePostRegionAsync(Guid postId, Guid regionId)
        {
            var kq = await _postService.DeletePostRegionAsync(postId, regionId);
            if (kq == true) return Ok(kq);
            return BadRequest(kq);
        }

        [HttpGet("detail/{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid Id)
        {
            return Ok(await _postService.GetById(Id));
        }

        [HttpPost("addcontent")]
        public async Task<IActionResult> AddPostContent([FromForm] CreateContentDto create)
        {
            var kq = await _postService.AddPostContent(create);
            if (kq == true) return Ok(kq);
            return BadRequest(kq);
        }
    }
}