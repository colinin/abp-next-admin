using LINGYUN.Abp.Elsa.Designer.Permissions;
using LINGYUN.Abp.Elsa.Localization;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.UI.Navigation;

namespace LINGYUN.Abp.Elsa.Designer.Navigation;

public class AbpElsaDesignerContributor : IMenuContributor
{
    public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return Task.CompletedTask;
        }
        var l = context.GetLocalizer<ElsaResource>();

        context.Menu.AddItem(
            new ApplicationMenuItem(
                AbpElsaDesignerMenuNames.Index,
                l["Elsa:Designer"],
                url: "~/Elsa",
                icon: "fa fa-code-fork",
                order: 1000, null)
            .RequirePermissions(AbpElsaDesignerPermissionsNames.View)
         );

        return Task.CompletedTask;
    }
}
