using System.ComponentModel.DataAnnotations;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class AggregateReRouteConfigCreateDto
    {
        [Required]
        public string RouteId { get; set; }

        [Required]
        [StringLength(256)]
        public string ReRouteKey { get; set; }

        [StringLength(1000)]
        public string Parameter { get; set; }

        [StringLength(256)]
        public string JsonPath { get; set; }
    }
}
