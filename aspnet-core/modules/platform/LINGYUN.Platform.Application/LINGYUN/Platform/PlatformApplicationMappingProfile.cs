using AutoMapper;
using LINGYUN.Platform.Versions;

namespace LINGYUN.Platform
{
    public class PlatformApplicationMappingProfile : Profile
    {
        public PlatformApplicationMappingProfile()
        {
            CreateMap<VersionFile, VersionFileDto>();
            CreateMap<AppVersion, VersionDto>();
        }
    }
}
