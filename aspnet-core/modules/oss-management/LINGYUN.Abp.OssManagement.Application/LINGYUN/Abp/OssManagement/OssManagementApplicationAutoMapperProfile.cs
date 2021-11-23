using AutoMapper;

namespace LINGYUN.Abp.OssManagement
{
    public class OssManagementApplicationAutoMapperProfile : Profile
    {
        public OssManagementApplicationAutoMapperProfile()
        {
            CreateMap<OssContainer, OssContainerDto>();
            CreateMap<OssObject, OssObjectDto>()
                .ForMember(dto => dto.Path, map => map.MapFrom(src => src.Prefix));

            CreateMap<GetOssContainersResponse, OssContainersResultDto>();
            CreateMap<GetOssObjectsResponse, OssObjectsResultDto>();

            CreateMap<FileShareCacheItem, MyFileShareDto>();
        }
    }
}
