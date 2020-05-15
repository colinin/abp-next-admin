using System.ComponentModel.DataAnnotations;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class AggregateRouteGetByAppIdInputDto
    {
        [Required]
        public string AppId { get; set; }
    }
}
