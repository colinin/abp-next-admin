using Localization.Resources.AbpUi;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace LINGYUN.Abp.MicroService.WorkflowService.Navigation;

public class UserMenuContributor : IMenuContributor
{
    public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.User)
        {
            return Task.CompletedTask;
        }

        var uiResource = context.GetLocalizer<AbpUiResource>();

        context.Menu.AddItem(new ApplicationMenuItem("Auth.Logout", uiResource["Menu:Logout"], url: "~/Account/Logout", icon: "fa fa-power-off", order: int.MaxValue - 1000));

        return Task.CompletedTask;
    }
}
