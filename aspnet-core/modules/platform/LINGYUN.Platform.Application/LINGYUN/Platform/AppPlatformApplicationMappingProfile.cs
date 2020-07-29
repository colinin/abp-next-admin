using AutoMapper;
using LINGYUN.Platform.Versions;

namespace LINGYUN.Platform
{
    public class AppPlatformApplicationMappingProfile : Profile
    {
        public AppPlatformApplicationMappingProfile()
        {
            CreateMap<VersionFile, VersionFileDto>();
            CreateMap<AppVersion, VersionDto>();
        }
    }
}
