using LINGYUN.Abp.BlobManagement;
using LINGYUN.Abp.UI.Navigation;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MicroService.PlatformService;
public class PlatformServiceDataSeeder : ITransientDependency
{
    protected NavigationDataSeeder NavigationDataSeeder { get; }
    protected IBlobDataSeeder BlobDataSeeder { get; }
    protected ICurrentTenant CurrentTenant { get; }
    public PlatformServiceDataSeeder(
        NavigationDataSeeder navigationDataSeeder, 
        ICurrentTenant currentTenant,
        IBlobDataSeeder blobDataSeeder)
    {
        NavigationDataSeeder = navigationDataSeeder;
        CurrentTenant = currentTenant;
        BlobDataSeeder = blobDataSeeder;
    }

    public async virtual Task SeedAsync(DataSeedContext context)
    {
        using (CurrentTenant.Change(context.TenantId))
        {
            await SeedNavigationAsync();
            await SeedBlobContainerAsync();
        }
    }

    private async Task SeedNavigationAsync()
    {
        await NavigationDataSeeder.SeedAsync(CurrentTenant.Id);
    }

    private async Task SeedBlobContainerAsync()
    {
        await BlobDataSeeder.SeedAsync(CurrentTenant.Id);
    }
}
