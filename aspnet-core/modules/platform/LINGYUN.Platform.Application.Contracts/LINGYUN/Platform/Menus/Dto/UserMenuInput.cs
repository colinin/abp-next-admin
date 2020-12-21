using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Platform.Menus
{
    public class UserMenuInput
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public List<Guid> MenuIds { get; set; } = new List<Guid>();
    }
}
