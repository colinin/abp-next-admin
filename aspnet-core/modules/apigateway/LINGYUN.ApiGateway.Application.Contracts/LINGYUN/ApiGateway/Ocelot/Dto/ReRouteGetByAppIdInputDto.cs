using System.ComponentModel.DataAnnotations;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class ReRouteGetByAppIdInputDto
    {
        [Required]
        [StringLength(50)]
        public string AppId { get; set; }
    }
}
