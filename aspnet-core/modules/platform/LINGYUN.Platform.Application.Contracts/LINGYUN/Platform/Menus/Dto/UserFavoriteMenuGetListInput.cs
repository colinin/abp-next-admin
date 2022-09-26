using LINGYUN.Platform.Routes;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Menus;
public class UserFavoriteMenuGetListInput
{
    [DynamicStringLength(typeof(LayoutConsts), nameof(LayoutConsts.MaxFrameworkLength))]
    public string Framework { get; set; }
}
