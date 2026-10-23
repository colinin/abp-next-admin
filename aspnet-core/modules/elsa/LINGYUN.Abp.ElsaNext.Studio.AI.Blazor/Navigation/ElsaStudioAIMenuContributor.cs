using LINGYUN.Abp.ElsaNext.Localization;
using LINGYUN.Abp.ElsaNext.Studio.Blazor.Navigation;
using Volo.Abp.UI.Navigation;

namespace LINGYUN.Abp.ElsaNext.Studio.AI.Blazor.Navigation;

public class ElsaStudioAIMenuContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            var menu = context.Menu;
            var l = context.GetLocalizer<ElsaNextResource>();

            var group = menu.GetMenuItemOrNull(ElsaStudioMenus.GroupName);
            group?.AddItem(new ApplicationMenuItem(
                ElsaStudioMenus.GroupName + ".AI",
                l["Menu:AI"],
                url: "/ai/weaver",
                icon: "fa fa-openai",
                order: 6));
        }

        return Task.CompletedTask;
    }
}
