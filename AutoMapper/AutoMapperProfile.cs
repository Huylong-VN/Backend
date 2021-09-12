using AutoMapper;
using Backend.Data.Entity;
using Backend.ViewModels.Categories;
using Backend.ViewModels.Common;
using Backend.ViewModels.Contents;
using Backend.ViewModels.Images;
using Backend.ViewModels.Posts;
using Backend.ViewModels.Regions;
using Backend.ViewModels.Users;
using System.Linq;

namespace Backend.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, UserVm>().ReverseMap();
            CreateMap<AppUser, CreateUserDto>().ReverseMap();
            CreateMap<AppUser, UpdateUserDto>().ReverseMap();
            CreateMap<Post, PostVm>().ForMember(x => x.Region, x => x.MapFrom(x => x.PostRegions.Select(x => x.Region))).ReverseMap();
            CreateMap<Post, CreatePostDto>().ReverseMap();
            CreateMap<Post, UpdatePostDto>();
            CreateMap<Category, CategoryVm>();
            CreateMap<CategoryVm, ListResultDto>();
            CreateMap<Image, ImageVm>().ReverseMap();
            CreateMap<Region, RegionVm>().ReverseMap();
            CreateMap<Region, CreateRegionDto>().ReverseMap();
            CreateMap<Region, UpdateRegionDto>().ReverseMap();
            CreateMap<Content, ContentVm>().ForMember(x => x.Images, x => x.MapFrom(x => x.Images)).ReverseMap();
        }
    }
}