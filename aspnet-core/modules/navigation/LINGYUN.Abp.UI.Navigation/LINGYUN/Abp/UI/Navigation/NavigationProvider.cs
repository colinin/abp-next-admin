using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.UI.Navigation
{
    public class NavigationProvider : INavigationProvider, ITransientDependency
    {
        protected INavigationDefinitionManager NavigationDefinitionManager { get; }
        public NavigationProvider(
            INavigationDefinitionManager navigationDefinitionManager)
        {
            NavigationDefinitionManager = navigationDefinitionManager;
        }
        public Task<IReadOnlyCollection<ApplicationMenu>> GetAllAsync()
        {
            var navigations = new List<ApplicationMenu>();
            var navigationDefineitions = NavigationDefinitionManager.GetAll();
            foreach (var navigationDefineition in navigationDefineitions)
            {
                navigations.Add(navigationDefineition.Menu);
            }

            IReadOnlyCollection<ApplicationMenu> menus = navigations.ToImmutableList();

            return Task.FromResult(menus);
        }
    }
}
