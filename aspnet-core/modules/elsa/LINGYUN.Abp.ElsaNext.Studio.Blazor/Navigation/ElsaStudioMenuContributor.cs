using LINGYUN.Abp.ElsaNext.Studio.Translations.Localization;
using Volo.Abp.UI.Navigation;

namespace LINGYUN.Abp.ElsaNext.Studio.Blazor.Navigation;

public class ElsaStudioMenuContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            AddStudioMenu(context);
        }

        return Task.CompletedTask;
    }

    private static void AddStudioMenu(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<ElsaStudioResource>();

        var elsaStudioMenu = new ApplicationMenuItem(
            ElsaStudioMenus.GroupName,
            l["Menu:ElsaStudio"],
            icon: "fa fa-diagram-project");

        elsaStudioMenu.AddItem(new ApplicationMenuItem(
            ElsaStudioMenus.GroupName + ".WorkflowDefinitions",
            l["Menu:WorkflowDefinitions"],
            url: "/workflows/definitions",
            icon: "fa fa-list",
            order: 1));
        elsaStudioMenu.AddItem(new ApplicationMenuItem(
            ElsaStudioMenus.GroupName + ".WorkflowInstances",
            l["Menu:WorkflowInstances"],
            url: "/workflows/instances",
            icon: "fa fa-cogs",
            order: 2));

        elsaStudioMenu.AddItem(new ApplicationMenuItem(
            ElsaStudioMenus.GroupName + ".Settings",
            l["Menu:Settings"],
            url: "/settings",
            icon: "fa fa-gear",
            order: 99));

        context.Menu.AddItem(elsaStudioMenu);
    }
}
