using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Platform.Menus
{
    public class MenuCreateDto : MenuCreateOrUpdateDto
    {
        [Required]
        public Guid LayoutId { get; set; }
    }
}
