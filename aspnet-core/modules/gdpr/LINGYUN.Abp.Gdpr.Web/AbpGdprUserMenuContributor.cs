using LINGYUN.Abp.Gdpr.Localization;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace LINGYUN.Abp.Gdpr.Web;

public class AbpGdprUserMenuContributor : IMenuContributor
{
    public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.User)
        {
            return Task.CompletedTask;
        }


        var gdprResource = context.GetLocalizer<GdprResource>();

        context.Menu.AddItem(
            new ApplicationMenuItem(
                "Account.Delete", 
                gdprResource["DeletePersonalAccount"], 
                url: "~/Account/Delete", 
                icon: "fa fa-remove", 
                order: int.MaxValue - 999));

        return Task.CompletedTask;
    }
}
