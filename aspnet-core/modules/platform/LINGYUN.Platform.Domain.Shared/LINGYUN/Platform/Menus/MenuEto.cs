using LINGYUN.Platform.Routes;
using Volo.Abp.EventBus;

namespace LINGYUN.Platform.Menus;

[EventName("platform.menus.menu")]
public class MenuEto : RouteEto
{
    public string Framework { get; set; }
}
