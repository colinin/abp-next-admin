using System.ComponentModel.DataAnnotations;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class GlobalGetByAppIdInputDto
    {
        [Required]
        public string AppId { get; set; }
    }
}
