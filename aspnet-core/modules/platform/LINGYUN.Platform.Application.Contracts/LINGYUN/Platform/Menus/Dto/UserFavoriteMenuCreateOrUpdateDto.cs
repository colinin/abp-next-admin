using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Menus;

public abstract class UserFavoriteMenuCreateOrUpdateDto
{
    [Required]
    public Guid MenuId { get; set; }

    [DynamicStringLength(typeof(UserFavoriteMenuConsts), nameof(UserFavoriteMenuConsts.MaxColorLength))]
    public string Color { get; set; }

    [DynamicStringLength(typeof(UserFavoriteMenuConsts), nameof(UserFavoriteMenuConsts.MaxAliasNameLength))]
    public string AliasName { get; set; }

    [DynamicStringLength(typeof(UserFavoriteMenuConsts), nameof(UserFavoriteMenuConsts.MaxIconLength))]
    public string Icon { get; set; }
}
