using Backend.ViewModels.Common;
using Backend.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces
{
    public interface IPostService
    {
        Task<PagedResultDto<PostVm>> GetListAsync(PagedAndSortedResultRequestDto request);

        Task<ApiResult<bool>> CreateAsync(CreatePostDto request);

        Task<PostVm> GetById(Guid Id);

        Task<bool> AddPostContent(CreateContentDto create);

        Task<ApiResult<PostVm>> UpdateAsync(Guid Id, UpdatePostDto request);

        Task<bool> DeleteAsync(Guid Id);

        Task<bool> DeletePostRegionAsync(Guid postId, Guid regionId);

        Task<List<ListResultDto>> GetListCategory();
    }
}