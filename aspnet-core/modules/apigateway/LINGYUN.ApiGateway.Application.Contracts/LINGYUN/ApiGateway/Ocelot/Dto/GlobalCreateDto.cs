using System.ComponentModel.DataAnnotations;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class GlobalCreateDto : GlobalConfigurationDtoBase
    {
        [Required]
        public string AppId { get; set; }
    }
}
