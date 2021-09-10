using AutoMapper;
using Backend.Application.Interfaces;
using Backend.Data.DbContext;
using Backend.Data.Entity;
using Backend.ViewModels.Common;
using Backend.ViewModels.Regions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Application.Implements
{
    public class RegionService : IRegionService
    {
        private readonly NewsAppDbContext _context;
        private readonly IMapper _mapper;

        public RegionService(NewsAppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> CreateAsync(CreateRegionDto request)
        {
            var region = _mapper.Map<Region>(request);
            region.CreateAt = DateTime.Now;
            _context.Regions.Add(region);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            var region = await _context.Regions.FindAsync(Id);
            if (region == null) return false;
            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<RegionVm>> GetListAsync(PagedAndSortedResultRequestDto request)
        {
            var region = _context.Regions;
            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                region.Where(x => x.Name.Contains(request.Filter));
            }
            if (string.IsNullOrEmpty(request.Sorting)) request.Sorting = nameof(Region.Name);
            if (request.SkipCount == 0) request.SkipCount = 1;
            if (request.MaxResultCount == 0) request.MaxResultCount = 10;
            var kq = await region.OrderBy(x => request.Sorting).Skip((request.SkipCount - 1) * request.MaxResultCount).Take(request.MaxResultCount).ToListAsync();
            return _mapper.Map<List<RegionVm>>(kq);
        }

        public async Task<bool> UpdateAsync(Guid Id, UpdateRegionDto request)
        {
            var region = await _context.Regions.FindAsync(Id);
            if (region == null) return false;
            region.UpdateAt = DateTime.Now;
            region.Name = request.Name;
            region.Description = request.Description;
            _context.Regions.Update(region);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}