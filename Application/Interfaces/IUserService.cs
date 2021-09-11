using Backend.ViewModels.Common;
using Backend.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces
{
    public interface IUserService
    {
        Task<ApiResult<UserAuthenticate>> Authenticate(UserLoginRequest request);

        Task<PagedResultDto<UserVm>> GetListAsync(PagedAndSortedResultRequestDto request);

        Task<ApiResult<bool>> CreateAsync(CreateUserDto request);

        Task<ApiResult<UserVm>> UpdateAsync(Guid Id, UpdateUserDto request);

        Task<bool> DeleteAsync(Guid Id);

        Task<bool> PermissionUser(Guid Id, string claimValue);
    }
}