using AutoMapper;

namespace LINGYUN.Abp.FileManagement
{
    public class FileManagementApplicationAutoMapperProfile : Profile
    {
        public FileManagementApplicationAutoMapperProfile()
        {
            CreateMap<OssContainer, OssContainerDto>();
            CreateMap<OssObject, OssObjectDto>()
                .ForMember(dto => dto.Path, map => map.MapFrom(src => src.Prefix));

            CreateMap<GetOssContainersResponse, OssContainersResultDto>();
            CreateMap<GetOssObjectsResponse, OssObjectsResultDto>();
        }
    }
}
