using Backend.ViewModels.Common;
using Backend.ViewModels.Images;
using Backend.ViewModels.Posts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces
{
    public interface IPostService
    {
        Task<PagedResultDto<PostVm>> GetListAsync(PagedAndSortedResultRequestDto request);

        Task<ApiResult<PostVm>> CreateAsync(CreatePostDto request);

        Task<PostVm> GetById(Guid Id);

        Task<bool> AddImagePostAsync(CreatePostImageDto create);

        Task<ApiResult<PostVm>> UpdateAsync(Guid Id, UpdatePostDto request);

        Task<bool> DeleteAsync(Guid Id);

        Task<bool> DeletePostRegionAsync(Guid postId, Guid regionId);

        Task<List<ListResultDto>> GetListCategory();

        //Task<bool> RemoveImagePost(Guid Id);
    }
}