using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Layouts
{
    public class GetLayoutListInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public bool Reverse { get; set; }

        public PlatformType? PlatformType { get; set; }
    }
}
