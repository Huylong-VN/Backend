using AutoMapper;
using Backend.Application.Interfaces;
using Backend.Data.DbContext;
using Backend.ViewModels.Common;
using Backend.ViewModels.Contents;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Application.Implements
{
    public class ContentService : IContentService
    {
        private readonly NewsAppDbContext _context;
        private readonly IMapper _mapper;

        public ContentService(IMapper mapper, NewsAppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ApiResult<ContentVm>> GetByIdAsync(Guid Id)
        {
            if (await _context.Contents.FindAsync(Id) == null) return new ApiErrorResult<ContentVm>("Khoong timf thay ");
            var content = await _context.Contents.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id.Equals(Id));
            var contentVm = _mapper.Map<ContentVm>(content);
            return new ApiSuccessResult<ContentVm>(contentVm);
        }

        public async Task<bool> RemoveImagePostContent(Guid Id)
        {
            var content = await _context.Contents.FindAsync(Id);
            if (content == null) return false;
            _context.Contents.Remove(content);
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
    }
}