using LINGYUN.Abp.ElsaNext.Studio.Blazor.Navigation;
using LINGYUN.Abp.ElsaNext.Studio.Translations.Localization;
using Volo.Abp.UI.Navigation;

namespace LINGYUN.Abp.ElsaNext.Studio.Diagnostics.StructuredLogs.Blazor.Navigation;

/// <summary>
/// Adds the structured-logs page to the "ElsaStudio" menu group (created by the Studio.Blazor
/// module; created here too when absent so this module stays self-contained).
/// </summary>
public class ElsaStudioStructuredLogsMenuContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            var menu = context.Menu;
            var l = context.GetLocalizer<ElsaStudioResource>();

            var group = menu.GetMenuItemOrNull(ElsaStudioMenus.GroupName);
            group?.AddItem(new ApplicationMenuItem(
                ElsaStudioMenus.GroupName + ".StructuredLogs",
                l["Menu:StructuredLogs"],
                url: "/diagnostics/structured-logs",
                icon: "fa fa-receipt",
                order: 4));
        }

        return Task.CompletedTask;
    }
}
