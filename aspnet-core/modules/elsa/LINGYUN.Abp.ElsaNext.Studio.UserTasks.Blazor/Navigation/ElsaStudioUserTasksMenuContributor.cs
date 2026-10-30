using LINGYUN.Abp.ElsaNext.Localization;
using LINGYUN.Abp.ElsaNext.Studio.Blazor.Navigation;
using Volo.Abp.UI.Navigation;

namespace LINGYUN.Abp.ElsaNext.Studio.UserTasks.Blazor.Navigation;

public class ElsaStudioUserTasksMenuContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            var menu = context.Menu;
            var l = context.GetLocalizer<ElsaNextResource>();

            var group = menu.GetMenuItemOrNull(ElsaStudioMenus.GroupName);
            group?.AddItem(new ApplicationMenuItem(
                ElsaStudioMenus.GroupName + ".UserTasks",
                l["Menu:UserTasks"],
                url: "/workflows/user-tasks",
                icon: "fa fa-list-check",
                order: 300));
        }

        return Task.CompletedTask;
    }
}
