using LINGYUN.Abp.ElsaNext.Studio.Blazor.Navigation;
using LINGYUN.Abp.ElsaNext.Studio.Translations.Localization;
using Volo.Abp.UI.Navigation;

namespace LINGYUN.Abp.ElsaNext.Studio.Diagnostics.OpenTelemetry.Blazor.Navigation;

public class ElsaStudioOpenTelemetryMenuContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            var menu = context.Menu;
            var l = context.GetLocalizer<ElsaStudioResource>();

            var group = menu.GetMenuItemOrNull(ElsaStudioMenus.GroupName);
            group?.AddItem(new ApplicationMenuItem(
                ElsaStudioMenus.GroupName + ".OpenTelemetry",
                l["Menu:OpenTelemetry"],
                url: "/diagnostics/opentelemetry",
                icon: "fa fa-x-ray",
                order: 4));
        }

        return Task.CompletedTask;
    }
}
