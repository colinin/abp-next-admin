using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.LocalizationManagement
{
    public class GetResourcesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
