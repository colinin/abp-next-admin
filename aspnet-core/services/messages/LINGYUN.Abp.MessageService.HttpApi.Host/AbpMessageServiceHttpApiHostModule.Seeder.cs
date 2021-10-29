using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.MessageService
{
    public partial class AbpMessageServiceHttpApiHostModule
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
}
