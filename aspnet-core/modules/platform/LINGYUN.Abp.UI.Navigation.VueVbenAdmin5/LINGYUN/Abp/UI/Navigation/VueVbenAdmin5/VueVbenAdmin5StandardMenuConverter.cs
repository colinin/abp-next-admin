using LINGYUN.Platform.Menus;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.UI.Navigation.VueVbenAdmin5;

[Dependency(ReplaceServices = true)]
public class VueVbenAdmin5StandardMenuConverter : IStandardMenuConverter, ISingletonDependency
{
    public StandardMenu Convert(Menu menu)
    {
        var standardMenu = new StandardMenu
        {
            Icon = "",
            Name = menu.Name,
            Path = menu.Path,
            DisplayName = menu.DisplayName,
            Description = menu.Description,
            Redirect = menu.Redirect,
        };

        if (menu.ExtraProperties.TryGetValue("icon", out var icon))
        {
            standardMenu.Icon = icon.ToString();
        }

        return standardMenu;
    }
}
