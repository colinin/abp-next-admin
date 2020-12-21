using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Platform.Menus
{
    public class RoleMenuInput
    {
        [Required]
        [StringLength(80)]
        public string RoleName { get; set; }

        [Required]
        public List<Guid> MenuIds { get; set; } = new List<Guid>();
    }
}
