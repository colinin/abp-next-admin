using LINGYUN.Abp.ElsaNext.Studio.Blazor.Navigation;
using LINGYUN.Abp.ElsaNext.Studio.Translations.Localization;
using Volo.Abp.UI.Navigation;

namespace LINGYUN.Abp.ElsaNext.Studio.Dashboard.Blazor.Navigation;

/// <summary>
/// Adds the dashboard page to the "ElsaStudio" menu group (created by the Studio.Blazor module;
/// created here too when absent so this module stays self-contained).
/// </summary>
public class ElsaStudioDashboardMenuContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            var menu = context.Menu;
            var l = context.GetLocalizer<ElsaStudioResource>();

            var group = menu.GetMenuItemOrNull(ElsaStudioMenus.GroupName);
            group?.AddItem(new ApplicationMenuItem(
                ElsaStudioMenus.GroupName + ".Dashboard",
                l["Menu:Dashboard"],
                url: "/dashboard",
                icon: "fa fa-chart-line",
                order: 0));
        }

        return Task.CompletedTask;
    }
}
