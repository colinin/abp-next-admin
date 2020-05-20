using System.ComponentModel.DataAnnotations;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class AggregateReRouteUpdateDto : AggregateReRouteDtoBase
    {
        [Required]
        public string RouteId { get; set; }

        [Required]
        public string ConcurrencyStamp { get; set; }
    }
}
