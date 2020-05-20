using System.ComponentModel.DataAnnotations;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class AggregateRouteGetByRouteIdInputDto
    {
        [Required]
        public string RouteId { get; set; }
    }
}
