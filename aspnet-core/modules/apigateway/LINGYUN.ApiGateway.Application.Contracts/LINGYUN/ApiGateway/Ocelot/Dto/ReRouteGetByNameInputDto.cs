using System.ComponentModel.DataAnnotations;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class ReRouteGetByNameInputDto
    {
        [Required]
        [StringLength(50)]
        public string RouteName { get; set; }
    }
}
