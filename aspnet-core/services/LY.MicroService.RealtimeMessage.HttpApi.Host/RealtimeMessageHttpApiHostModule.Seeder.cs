using LY.MicroService.RealtimeMessage.DataSeeder;
using Microsoft.Extensions.DependencyInjection;

namespace LY.MicroService.RealtimeMessage;

public partial class RealtimeMessageHttpApiHostModule
{
    private static void ConfigureSeedWorker(IServiceCollection services, bool isDevelopment = false)
    {
        if (isDevelopment)
        {
            services.AddHostedService<RealtimeMessageDataSeederWorker>();
        }
    }
}
