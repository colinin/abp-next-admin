using System.ComponentModel.DataAnnotations;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class RouteGroupUpdateDto
    {
        [Required]
        [StringLength(50)]
        public string AppId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
