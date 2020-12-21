using AutoMapper;
using LINGYUN.Platform.Routes;
using LINGYUN.Platform.Versions;

namespace LINGYUN.Platform
{
    public class PlatformDomainMappingProfile : Profile
    {
        public PlatformDomainMappingProfile()
        {
            CreateMap<Route, RouteEto>();

            CreateMap<AppVersion, AppVersionEto>()
                .ForMember(eto => eto.FileCount, map => map.MapFrom(src => src.Files.Count));
        }
    }
}
