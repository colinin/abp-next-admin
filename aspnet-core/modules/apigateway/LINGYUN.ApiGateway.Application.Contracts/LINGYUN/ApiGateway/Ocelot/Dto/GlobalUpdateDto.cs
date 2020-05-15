using System.ComponentModel.DataAnnotations;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class GlobalUpdateDto : GlobalConfigurationDtoBase
    {
        [Required]
        public long ItemId { get; set; }
    }
}
