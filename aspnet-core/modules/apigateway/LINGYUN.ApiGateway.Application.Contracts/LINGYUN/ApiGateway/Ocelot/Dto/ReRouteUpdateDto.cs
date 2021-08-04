using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class ReRouteUpdateDto : ReRouteDtoBase
    {
        [Required]
        public string ReRouteId { get; set; }

        [Required]
        public string ConcurrencyStamp { get; set; }

        [StringLength(50)]
        public string ReRouteName { get; set; }
    }
}
