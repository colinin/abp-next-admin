using LINGYUN.Abp.MessageService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.Threading;

namespace LY.MicroService.RealtimeMessage;

public partial class RealtimeMessageHttpApiHostModule
{
    private void SeedData(ApplicationInitializationContext context)
    {
        if (context.GetEnvironment().IsDevelopment())
        {
            AsyncHelper.RunSync(async () =>
                await context.ServiceProvider.GetRequiredService<IMessageDataSeeder>()
                    .SeedAsync());
        }
    }
}
