using LINGYUN.Abp.ElsaNext.Localization;
using LINGYUN.Abp.ElsaNext.Studio.Blazor.Navigation;
using Volo.Abp.UI.Navigation;

namespace LINGYUN.Abp.ElsaNext.Studio.Labels.Blazor.Navigation;

public class ElsaStudioLabelsMenuContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            var menu = context.Menu;
            var l = context.GetLocalizer<ElsaNextResource>();

            var group = menu.GetMenuItemOrNull(ElsaStudioMenus.GroupName);
            group?.AddItem(new ApplicationMenuItem(
                ElsaStudioMenus.GroupName + ".Labels",
                l["Menu:Labels"],
                url: "/labels",
                icon: "fa fa-tags",
                order: 200));
        }

        return Task.CompletedTask;
    }
}
