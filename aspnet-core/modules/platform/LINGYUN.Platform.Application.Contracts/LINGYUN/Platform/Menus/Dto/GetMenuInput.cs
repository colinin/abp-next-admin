using LINGYUN.Platform.Routes;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Menus;

public class GetMenuInput
{
    [DynamicStringLength(typeof(LayoutConsts), nameof(LayoutConsts.MaxFrameworkLength))]
    public string Framework { get; set; }
}
