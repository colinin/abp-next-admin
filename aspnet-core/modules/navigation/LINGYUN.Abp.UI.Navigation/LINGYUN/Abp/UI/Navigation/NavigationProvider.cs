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
            var navigationDefinitions = NavigationDefinitionManager.GetAll();
            foreach (var navigationDefinition in navigationDefinitions)
            {
                navigations.Add(navigationDefinition.Menu);
            }

            IReadOnlyCollection<ApplicationMenu> menus = navigations.ToImmutableList();

            return Task.FromResult(menus);
        }
    }
}
