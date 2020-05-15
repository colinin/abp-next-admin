using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class AggregateRouteGetByPagedInputDto : PagedAndSortedResultRequestDto
    {
        [Required]
        [StringLength(50)]
        public string AppId { get; set; }

        public string Filter { get; set; }
    }
}
