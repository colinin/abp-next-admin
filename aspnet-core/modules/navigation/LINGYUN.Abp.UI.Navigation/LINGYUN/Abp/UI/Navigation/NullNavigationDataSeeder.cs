using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.UI.Navigation
{
    [Dependency(TryRegister = true)]
    public class NullNavigationDataSeeder : INavigationDataSeeder, ISingletonDependency
    {
        public Task SeedAsync(
            IReadOnlyCollection<ApplicationMenu> menus,
            MultiTenancySides multiTenancySides = MultiTenancySides.Both)
        {
            return Task.CompletedTask;
        }
    }
}
