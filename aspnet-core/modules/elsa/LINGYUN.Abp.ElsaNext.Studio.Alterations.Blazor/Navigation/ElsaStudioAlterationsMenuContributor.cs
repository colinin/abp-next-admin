using LINGYUN.Abp.ElsaNext.Localization;
using LINGYUN.Abp.ElsaNext.Studio.Blazor.Navigation;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace LINGYUN.Abp.ElsaNext.Studio.Alterations.Blazor.Navigation;

public class ElsaStudioAlterationsMenuContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            var menu = context.Menu;
            var l = context.GetLocalizer<ElsaNextResource>();

            var group = menu.GetMenuItemOrNull(ElsaStudioMenus.GroupName);
            group?.AddItem(new ApplicationMenuItem(
                ElsaStudioMenus.GroupName + ".Alterations",
                l["Menu:Alterations"],
                url: "/alterations",
                icon: "fa fa-clock-rotate-left",
                order: 5));
        }

        return Task.CompletedTask;
    }
}
