using LINGYUN.Abp.UI.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MicroService.PlatformService;
public class PlatformServiceDataSeeder : ITransientDependency
{
    protected IEnumerable<INavigationSeedContributor> NavigationSeedContributors { get; }
    protected INavigationProvider NavigationProvider { get; }
    protected ICurrentTenant CurrentTenant { get; }
    public PlatformServiceDataSeeder(
        IEnumerable<INavigationSeedContributor> navigationSeedContributors, 
        INavigationProvider navigationProvider, 
        ICurrentTenant currentTenant)
    {
        NavigationSeedContributors = navigationSeedContributors;
        NavigationProvider = navigationProvider;
        CurrentTenant = currentTenant;
    }

    public async virtual Task SeedAsync(DataSeedContext context)
    {
        using (CurrentTenant.Change(context.TenantId))
        {
            await SeedNavigationAsync();
        }
    }

    private async Task SeedNavigationAsync()
    {
        var menus = await NavigationProvider.GetAllAsync();

        var multiTenancySides = CurrentTenant.IsAvailable
            ? MultiTenancySides.Tenant
            : MultiTenancySides.Host;

        var seedContext = new NavigationSeedContext(menus, multiTenancySides);

        foreach (var contributor in NavigationSeedContributors)
        {
            await contributor.SeedAsync(seedContext);
        }
    }
}
