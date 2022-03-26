using Microsoft.Extensions.DependencyInjection;
using LY.MicroService.WebhooksManagement.DataSeeder;

namespace LY.MicroService.WebhooksManagement;

public partial class WebhooksManagementHttpApiHostModule
{
    private static void ConfigureSeedWorker(IServiceCollection services, bool isDevelopment = false)
    {
        if (isDevelopment)
        {
            services.AddHostedService<WebhooksManagementDataSeederWorker>();
        }
    }
}
