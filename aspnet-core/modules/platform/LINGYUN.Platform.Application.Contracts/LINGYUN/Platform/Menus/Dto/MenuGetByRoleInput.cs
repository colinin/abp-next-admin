using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Platform.Menus
{
    public class MenuGetByRoleInput
    {
        [Required]
        [StringLength(80)]
        public string Role { get; set; }

        public PlatformType PlatformType { get; set; }
    }
}
