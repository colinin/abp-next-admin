using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.UI.Navigation
{
    public class NavigationDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly INavigationProvider _navigationProvider;
        private readonly INavigationDataSeeder _navigationDataSeeder;

        public NavigationDataSeedContributor(
            ICurrentTenant currentTenant,
            INavigationProvider navigationProvider,
            INavigationDataSeeder navigationDataSeeder)
        {
            _currentTenant = currentTenant;
            _navigationProvider = navigationProvider;
            _navigationDataSeeder = navigationDataSeeder;
        }

        public virtual async Task SeedAsync(DataSeedContext context)
        {
            using(_currentTenant.Change(context.TenantId))
            {
                var multiTenancySides = _currentTenant.Id.HasValue
                    ? MultiTenancySides.Tenant
                    : MultiTenancySides.Host;

                var menus = await _navigationProvider.GetAllAsync();

                await _navigationDataSeeder.SeedAsync(menus, multiTenancySides);
            }
        }
    }
}
