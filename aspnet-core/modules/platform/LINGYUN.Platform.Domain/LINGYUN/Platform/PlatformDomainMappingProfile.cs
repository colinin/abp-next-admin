using AutoMapper;
using LINGYUN.Platform.Layouts;
using LINGYUN.Platform.Menus;
using LINGYUN.Platform.Routes;
using LINGYUN.Platform.Versions;

namespace LINGYUN.Platform
{
    public class PlatformDomainMappingProfile : Profile
    {
        public PlatformDomainMappingProfile()
        {
            CreateMap<Layout, LayoutEto>();

            CreateMap<Menu, MenuEto>();
            CreateMap<UserMenu, UserMenuEto>();
            CreateMap<RoleMenu, RoleMenuEto>();

            CreateMap<AppVersion, AppVersionEto>()
                .ForMember(eto => eto.FileCount, map => map.MapFrom(src => src.Files.Count));
        }
    }
}
