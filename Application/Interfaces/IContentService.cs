using Backend.ViewModels.Common;
using Backend.ViewModels.Contents;
using System;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces
{
    public interface IContentService
    {
        Task<ApiResult<ContentVm>> GetByIdAsync(Guid Id);

        Task<bool> RemoveImagePostContent(Guid Id);

        Task<bool> UpdateContentAsync(UpdateContentDto request);
    }
}