using AutoMapper;
using LINGYUN.Platform.Routes;
using LINGYUN.Platform.Versions;

namespace LINGYUN.Platform
{
    public class AppPlatformDomainMappingProfile : Profile
    {
        public AppPlatformDomainMappingProfile()
        {
            CreateMap<Route, RouteEto>();
            CreateMap<UserRoute, UserRouteEto>();
            CreateMap<RoleRoute, RoleRouteEto>();

            CreateMap<AppVersion, AppVersionEto>()
                .ForMember(eto => eto.FileCount, map => map.MapFrom(src => src.Files.Count));
        }
    }
}
