using System.ComponentModel.DataAnnotations;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class RouteGroupCreateDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string AppId { get; set; }

        [Required]
        [StringLength(100)]
        public string AppName { get; set; }

        [Required]
        [StringLength(256)]
        public string AppIpAddress { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
