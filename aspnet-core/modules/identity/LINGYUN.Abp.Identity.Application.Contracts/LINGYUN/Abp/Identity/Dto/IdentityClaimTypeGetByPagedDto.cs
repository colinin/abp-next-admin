using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Identity
{
    public class IdentityClaimTypeGetByPagedDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
