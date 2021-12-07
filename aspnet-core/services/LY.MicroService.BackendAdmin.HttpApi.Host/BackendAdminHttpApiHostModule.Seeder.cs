using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Threading;

namespace LY.MicroService.BackendAdmin;

public partial class BackendAdminHttpApiHostModule
{
    private static void SeedData(ApplicationInitializationContext context)
    {
        if (context.GetEnvironment().IsDevelopment())
        {
            AsyncHelper.RunSync(async () =>
            {
                using var scope = context.ServiceProvider.CreateScope();
                await scope.ServiceProvider.GetRequiredService<IDataSeeder>().SeedAsync();
            });
        }
    }
}
