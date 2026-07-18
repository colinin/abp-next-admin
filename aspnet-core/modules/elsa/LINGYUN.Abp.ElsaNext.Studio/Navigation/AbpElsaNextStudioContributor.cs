using LINGYUN.Abp.ElsaNext.Localization;
using LINGYUN.Abp.ElsaNext.Studio.Permissions;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.UI.Navigation;

namespace LINGYUN.Abp.ElsaNext.Studio.Navigation;

public class AbpElsaNextStudioContributor : IMenuContributor
{
    public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return Task.CompletedTask;
        }
        var l = context.GetLocalizer<ElsaNextResource>();

        context.Menu.AddItem(
            new ApplicationMenuItem(
                AbpElsaNextStudioMenuNames.Index,
                l["ElsaNext:Studio"],
                url: "~/ElsaStudio",
                icon: "fa fa-code-fork",
                order: 1000, null)
            //.RequirePermissions(AbpElsaNextStudioPermissionsNames.View)
         );

        return Task.CompletedTask;
    }
}
