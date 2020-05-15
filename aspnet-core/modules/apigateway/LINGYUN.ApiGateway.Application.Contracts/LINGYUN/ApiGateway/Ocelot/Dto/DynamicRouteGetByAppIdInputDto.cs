using System.ComponentModel.DataAnnotations;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class DynamicRouteGetByAppIdInputDto
    {
        [Required]
        public string AppId { get; set; }
    }
}
