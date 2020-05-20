using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.TenantManagement
{
    public class TenantGetByPagedInputDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}