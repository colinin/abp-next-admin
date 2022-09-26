using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Platform.Menus;
public class UserFavoriteMenuRemoveInput
{
    [Required]
    public Guid MenuId { get; set; }
}
