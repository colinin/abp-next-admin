using LINGYUN.Platform.Routes;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Menus;

public class UserFavoriteMenuCreateDto : UserFavoriteMenuCreateOrUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(LayoutConsts), nameof(LayoutConsts.MaxFrameworkLength))]

    public string Framework { get; set; }
}
