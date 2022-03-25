using Microsoft.Extensions.DependencyInjection;
using LY..WebhooksManagement.DataSeeder;

namespace LY..WebhooksManagement;

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
