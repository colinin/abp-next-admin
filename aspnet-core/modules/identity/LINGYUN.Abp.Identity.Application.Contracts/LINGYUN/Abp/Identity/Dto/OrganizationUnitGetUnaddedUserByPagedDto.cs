using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Identity
{
    public class OrganizationUnitGetUnaddedUserByPagedDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
