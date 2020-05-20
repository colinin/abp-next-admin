using System.ComponentModel.DataAnnotations;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class AggregateReRouteConfigGetByKeyInputDto
    {
        [Required]
        public string RouteId { get; set; }

        [Required]
        [StringLength(256)]
        public string ReRouteKey { get; set; }
    }
}
