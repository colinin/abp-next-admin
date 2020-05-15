using System.ComponentModel.DataAnnotations;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class ReRouteCreateDto : ReRouteDtoBase
    {
        [Required]
        [StringLength(50)]
        public string ReRouteName { get; set; }

        [Required]
        [StringLength(50)]
        public string AppId { get; set; }
    }
}
