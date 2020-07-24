using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Versions
{
    public class VersionGetByPagedDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public PlatformType PlatformType { get; set; } = PlatformType.None;
    }
}
