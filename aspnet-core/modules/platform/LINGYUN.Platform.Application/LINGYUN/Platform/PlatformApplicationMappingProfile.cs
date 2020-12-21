using AutoMapper;
using LINGYUN.Platform.Datas;
using LINGYUN.Platform.Layouts;
using LINGYUN.Platform.Menus;
using LINGYUN.Platform.Versions;

namespace LINGYUN.Platform
{
    public class PlatformApplicationMappingProfile : Profile
    {
        public PlatformApplicationMappingProfile()
        {
            CreateMap<VersionFile, VersionFileDto>();
            CreateMap<AppVersion, VersionDto>();

            CreateMap<DataItem, DataItemDto>();
            CreateMap<Data, DataDto>();
            CreateMap<Menu, MenuDto>()
                .ForMember(dto => dto.Meta, map => map.MapFrom(src => src.ExtraProperties));
            CreateMap<Layout, LayoutDto>()
                .ForMember(dto => dto.Meta, map => map.MapFrom(src => src.ExtraProperties));
        }
    }
}
