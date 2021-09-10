using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Backend.Application.Interfaces;
using Backend.Data.Entity;
using Backend.ViewModels.Common;
using Backend.ViewModels.Users;

namespace Backend.Application.Implements
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;

        public UserService(IConfiguration configuration, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper)
        {
            _mapper = mapper;
            _signInManager = signInManager;
            _configuration = configuration;
            _userManager = userManager;
        }

        private string GenerateToken(AppUser user)
        {
            var claims = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                 new Claim(ClaimTypes.Email, user.Email),
                 new Claim(ClaimTypes.Name, user.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["JwtConfig:Issuer"],
            _configuration["JwtConfig:Audience"], claims, expires: DateTime.Now.AddHours(3), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ApiResult<UserAuthenticate>> Authenticate(UserLoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return new ApiErrorResult<UserAuthenticate>("Can't find user in system");
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, true);
            if (!result.Succeeded) return new ApiErrorResult<UserAuthenticate>("Password Wrong");
            return new ApiSuccessResult<UserAuthenticate>(new UserAuthenticate
            {
                AccessToken = GenerateToken(user),
                Address = user.Address,
                Email = user.Email,
                FullName = user.FullName,
                Id = user.Id,
                ImagePath = user.ImagePath,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName
            }
            );
        }

        public async Task<ApiResult<bool>> CreateAsync(CreateUserDto request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null) return new ApiErrorResult<bool>("Tai khoan da ton tai");
            await _userManager.CreateAsync(_mapper.Map<AppUser>(request), request.Password);
            return new ApiSuccessResult<bool>();
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null) return false;
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<PagedResultDto<UserVm>> GetListAsync(PagedAndSortedResultRequestDto request)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query = query.Where(x => x.UserName.Contains(request.Filter) || x.Email.Contains(request.Filter));
            }
            if (string.IsNullOrEmpty(request.Sorting)) request.Sorting = nameof(AppUser.UserName);
            if (request.SkipCount == 0) request.SkipCount = 1;
            if (request.MaxResultCount == 0) request.MaxResultCount = 10;
            var t = await query.OrderBy(x => x.Id).Skip((request.SkipCount - 1) * request.MaxResultCount).Take(request.MaxResultCount).ToListAsync();
            var user = _mapper.Map<List<UserVm>>(t); ;
            return new PagedResultDto<UserVm> { Items = user, totalCount = t.Count };
        }

        public async Task<ApiResult<UserVm>> UpdateAsync(Guid Id, UpdateUserDto request)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null) return new ApiErrorResult<UserVm>("Tafi khoan khong ton tai");
            user.FullName = request.FullName;
            user.PhoneNumber = request.PhoneNumber;
            user.ImagePath = request.ImagePath;
            user.Email = request.Email;
            user.Address = request.Address;
            await _userManager.UpdateAsync(user);
            return new ApiSuccessResult<UserVm>(_mapper.Map<UserVm>(user));
        }
    }
}