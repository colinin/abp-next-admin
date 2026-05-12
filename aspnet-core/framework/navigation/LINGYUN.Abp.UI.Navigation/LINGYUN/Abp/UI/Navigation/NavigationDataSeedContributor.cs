using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.UI.Navigation;

public class NavigationDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly NavigationDataSeeder _navigationDataSeeder;

    public NavigationDataSeedContributor(
        NavigationDataSeeder navigationDataSeeder)
    {
        _navigationDataSeeder = navigationDataSeeder;
    }

    public async virtual Task SeedAsync(DataSeedContext context)
    {
        await _navigationDataSeeder.SeedAsync(context.TenantId);
    }
}
