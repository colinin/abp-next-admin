using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.UI.Navigation
{
    public class NavigationDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICurrentTenant _currentTenant;
        private readonly INavigationProvider _navigationProvider;

        private readonly AbpNavigationOptions _options;

        public List<INavigationSeedContributor> Contributors => _lazyContributors.Value;
        private readonly Lazy<List<INavigationSeedContributor>> _lazyContributors;

        public NavigationDataSeedContributor(
            IServiceProvider serviceProvider,
            ICurrentTenant currentTenant,
            INavigationProvider navigationProvider,
            IOptions<AbpNavigationOptions> options)
        {
            _serviceProvider = serviceProvider;
            _currentTenant = currentTenant;
            _navigationProvider = navigationProvider;

            _options = options.Value;
            _lazyContributors = new Lazy<List<INavigationSeedContributor>>(CreateContributors);
        }

        public virtual async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context.TenantId))
            {
                var menus = await _navigationProvider.GetAllAsync();

                var multiTenancySides = _currentTenant.IsAvailable
                    ? MultiTenancySides.Tenant
                    : MultiTenancySides.Host;

                var seedContext = new NavigationSeedContext(menus, multiTenancySides);

                foreach (var contributor in Contributors)
                {
                    await contributor.SeedAsync(seedContext);
                }
            }
        }

        private List<INavigationSeedContributor> CreateContributors()
        {
            return _options
                .NavigationSeedContributors
                .Select(type => _serviceProvider.GetRequiredService(type) as INavigationSeedContributor)
                .ToList();
        }
    }
}
