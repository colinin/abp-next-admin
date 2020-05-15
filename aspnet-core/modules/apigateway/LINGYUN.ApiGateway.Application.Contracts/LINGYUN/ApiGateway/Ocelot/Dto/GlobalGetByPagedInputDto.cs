using Volo.Abp.Application.Dtos;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class GlobalGetByPagedInputDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
