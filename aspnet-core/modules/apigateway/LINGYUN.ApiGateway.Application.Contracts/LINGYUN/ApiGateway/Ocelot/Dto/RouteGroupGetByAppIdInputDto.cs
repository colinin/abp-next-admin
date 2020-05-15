using System.ComponentModel.DataAnnotations;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class RouteGroupGetByAppIdInputDto
    {
        [Required]
        public string AppId { get; set; }
    }
}
