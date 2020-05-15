using Volo.Abp.Application.Dtos;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class RouteGroupGetByPagedInputDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
