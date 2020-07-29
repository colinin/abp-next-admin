using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Identity
{
    public class OrganizationUnitGetByPagedDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
