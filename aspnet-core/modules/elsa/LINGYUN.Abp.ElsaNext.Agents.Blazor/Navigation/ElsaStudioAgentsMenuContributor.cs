using LINGYUN.Abp.ElsaNext.Studio.Blazor.Navigation;
using LINGYUN.Abp.ElsaNext.Studio.Translations.Localization;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace LINGYUN.Abp.ElsaNext.Agents.Blazor.Navigation;

public class ElsaStudioAgentsMenuContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            var menu = context.Menu;
            var l = context.GetLocalizer<ElsaStudioResource>();

            var group = menu.GetMenuItemOrNull(ElsaStudioMenus.GroupName);
            group?.AddItem(new ApplicationMenuItem(
                ElsaStudioMenus.GroupName + ".Agents",
                l["Menu:Agents"],
                url: "/ai/agents",
                icon: "fa fa-bots",
                order: 5));
        }

        return Task.CompletedTask;
    }
}
