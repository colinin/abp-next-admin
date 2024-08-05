using AutoMapper;
using LINGYUN.Platform.Layouts;
using LINGYUN.Platform.Menus;
using LINGYUN.Platform.Packages;

namespace LINGYUN.Platform;

public class PlatformDomainMappingProfile : Profile
{
    public PlatformDomainMappingProfile()
    {
        CreateMap<Layout, LayoutEto>();

        CreateMap<Menu, MenuEto>();
        CreateMap<UserMenu, UserMenuEto>();
        CreateMap<RoleMenu, RoleMenuEto>();

        CreateMap<Package, PackageEto>();
    }
}
