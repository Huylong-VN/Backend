using Backend.ViewModels.Common;
using Backend.ViewModels.Regions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces
{
    public interface IRegionService
    {
        Task<List<RegionVm>> GetListAsync(PagedAndSortedResultRequestDto request);

        Task<bool> CreateAsync(CreateRegionDto request);

        Task<bool> UpdateAsync(Guid id, UpdateRegionDto request);

        Task<bool> DeleteAsync(Guid Id);
    }
}