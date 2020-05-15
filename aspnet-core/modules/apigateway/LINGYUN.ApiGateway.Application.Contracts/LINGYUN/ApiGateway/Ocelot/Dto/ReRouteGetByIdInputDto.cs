using System.ComponentModel.DataAnnotations;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class ReRouteGetByIdInputDto
    {
        [Required]
        public long RouteId { get; set; }
    }
}
