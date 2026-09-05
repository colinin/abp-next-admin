using LINGYUN.Abp.ElsaNext.Studio.Blazor.Navigation;
using LINGYUN.Abp.ElsaNext.Studio.Translations.Localization;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace LINGYUN.Abp.ElsaNext.Secrets.Blazor.Navigation;

public class ElsaStudioSecretsMenuContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            var menu = context.Menu;
            var l = context.GetLocalizer<ElsaStudioResource>();

            var group = menu.GetMenuItemOrNull(ElsaStudioMenus.GroupName);
            group?.AddItem(new ApplicationMenuItem(
                ElsaStudioMenus.GroupName + ".Secrets",
                l["Menu:Secrets"],
                url: "/security/secrets",
                icon: "fa fa-key",
                order: 6));
        }

        return Task.CompletedTask;
    }
}
