using System.Collections.Generic;

namespace LINGYUN.Abp.UI.Navigation
{
    public interface INavigationDefinitionManager
    {
        IReadOnlyList<NavigationDefinition> GetAll();
    }
}
