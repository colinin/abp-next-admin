using System.ComponentModel.DataAnnotations;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class AggregateReRouteCreateDto : AggregateReRouteDtoBase
    {
        [Required]
        [StringLength(50)]
        public string AppId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
