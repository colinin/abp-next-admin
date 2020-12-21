using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Platform.Menus
{
    public class MenuGetByUserInput
    {
        [Required]
        public Guid UserId { get; set; }

        public string[] Roles { get; set; } = new string[0];

        public PlatformType PlatformType { get; set; }
    }
}
