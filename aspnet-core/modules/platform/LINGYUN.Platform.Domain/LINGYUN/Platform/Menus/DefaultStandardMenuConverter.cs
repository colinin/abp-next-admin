using Volo.Abp.DependencyInjection;

namespace LINGYUN.Platform.Menus;

[Dependency(TryRegister = true)]
public class DefaultStandardMenuConverter : IStandardMenuConverter, ISingletonDependency
{
    public StandardMenu Convert(Menu menu)
    {
        return new StandardMenu
        {
            Icon = "",
            Name = menu.Name,
            Path = menu.Path,
            DisplayName = menu.DisplayName,
            Description = menu.Description,
            Redirect = menu.Redirect,
        };
    }
}
