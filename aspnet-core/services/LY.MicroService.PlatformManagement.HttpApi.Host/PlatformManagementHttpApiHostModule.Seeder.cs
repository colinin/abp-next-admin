using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Threading;

namespace LY.MicroService.PlatformManagement;

public partial class PlatformManagementHttpApiHostModule
{
    private void SeedData(ApplicationInitializationContext context)
    {
        if (context.GetEnvironment().IsDevelopment())
        {
            AsyncHelper.RunSync(async () =>
                await context.ServiceProvider.GetRequiredService<IDataSeeder>()
                    .SeedAsync());
        }
    }
}
