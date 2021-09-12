using AutoMapper;
using Backend.Application.Interfaces;
using Backend.Data.DbContext;
using Backend.Data.Entity;
using Backend.ViewModels.Categories;
using Backend.ViewModels.Common;
using Backend.ViewModels.Contents;
using Backend.ViewModels.Images;
using Backend.ViewModels.Posts;
using Backend.ViewModels.Regions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Backend.Application.Implements
{
    public class PostService : IPostService
    {
        private readonly NewsAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly IStorageService _storageService;

        public PostService(ICategoryService categoryService, NewsAppDbContext context, IMapper mapper, IStorageService storageService)
        {
            _storageService = storageService;
            _categoryService = categoryService;
            _mapper = mapper;
            _context = context;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<ApiResult<bool>> CreateAsync(CreatePostDto request)
        {
            var post = new Post()
            {
                CreateAt = DateTime.Now,
                CategoryId = request.CategoryId,
                Title = request.Title
            };
            _context.Posts.Add(post);
            var postContent = new Content()
            {
                PostId = post.Id,
                ContentItem = request.Content,
                CreateAt = DateTime.Now,
            };
            _context.Contents.Add(postContent);
            if (request.Image != null)
            {
                var image = new Image()
                {
                    ContentId = postContent.Id,
                    CreateAt = DateTime.Now,
                    Path = await SaveFile(request.Image),
                    Size = request.Image.Length,
                    isDefault = true
                };
                _context.Images.Add(image);
            }

            foreach (var regionId in request.RegionId)
            {
                var pr = new PostRegion() { PostId = post.Id, RegionId = regionId };

                _context.PostRegions.Add(pr);

                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            var post = await _context.Posts.FindAsync(Id);
            if (post == null) return false;
            var images = _context.Images.Where(x => x.ContentId.Equals(Id));
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.Path);
                _context.Images.Remove(image);
            }
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResultDto<PostVm>> GetListAsync(PagedAndSortedResultRequestDto request)
        {
            var query = _context.Posts.Include(x => x.Contents).ThenInclude(x => x.Images).Include(x => x.Category).Include(x => x.PostRegions).ThenInclude(x => x.Region);
            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query.Where(x => x.Title.Contains(request.Filter));
            }
            if (string.IsNullOrEmpty(request.Sorting)) request.Sorting = nameof(Post.CreateAt);
            if (request.SkipCount == 0) request.SkipCount = 1;
            if (request.MaxResultCount == 0) request.MaxResultCount = 10;
            var t = await query.OrderByDescending(x => x.CreateAt)
                .Skip((request.SkipCount - 1) * request.MaxResultCount).Take(request.MaxResultCount).AsSplitQuery().ToListAsync();
            var post = _mapper.Map<List<PostVm>>(t);
            return new PagedResultDto<PostVm> { Items = post, totalCount = t.Count };
        }

        public async Task<List<ListResultDto>> GetListCategory()
        {
            var result = await _categoryService.GetListAsync();
            return _mapper.Map<List<ListResultDto>>(result);
        }

        public async Task<ApiResult<PostVm>> UpdateAsync(Guid Id, UpdatePostDto request)
        {
            var post = await _context.Posts.FindAsync(Id);
            if (post == null) return new ApiErrorResult<PostVm>("Khong tim thay with Id:" + Id);
            post.Title = request.Title;
            post.CategoryId = request.CategoryId;
            post.UpdateAt = DateTime.Now;

            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<PostVm>(_mapper.Map<PostVm>(post));
        }

        public async Task<bool> DeletePostRegionAsync(Guid postId, Guid regionId)
        {
            var result = await _context.PostRegions.FindAsync(postId, regionId);
            if (result == null) return false;
            _context.PostRegions.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PostVm> GetById(Guid Id)
        {
            var query = await _context.Posts.Include(x => x.Contents)
                .Include(x => x.Category)
                .Include(x => x.PostRegions).ThenInclude(x => x.Region)
                .Where(x => x.Id.Equals(Id)).AsSplitQuery().FirstOrDefaultAsync();
            return _mapper.Map<PostVm>(query);
        }

        public async Task<bool> AddPostContent(CreateContentDto create)
        {
            var post = await _context.Posts.FindAsync(create.postId);
            if (post == null) return false;

            var postContent = new Content()
            {
                PostId = create.postId,
                ContentItem = create.content,
                CreateAt = DateTime.Now,
            };
            _context.Contents.Add(postContent);
            if (create.image != null)
            {
                foreach (var img in create.image)
                {
                    var image = new Image()
                    {
                        ContentId = postContent.Id,
                        CreateAt = DateTime.Now,
                        Path = await SaveFile(img),
                        Size = img.Length,
                    };
                    _context.Images.Add(image);
                }
            };
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveImagePostContent(Guid Id)
        {
            var image = await _context.Images.FindAsync(Id);
            if (image == null) return false;
            _context.Images.Remove(image);
            await _storageService.DeleteFileAsync(image.Path);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateContentAsync(UpdateContentDto request)
        {
            var content = await _context.Contents.FindAsync(request.ContentId);
            if (content == null) return false;
            content.ContentItem = request.ContentItem;
            _context.Contents.Update(content);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteContentAsync(Guid Id)
        {
            var content = await _context.Contents.FindAsync(Id);
            if (content == null) return false;
            _context.Contents.Remove(content);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}