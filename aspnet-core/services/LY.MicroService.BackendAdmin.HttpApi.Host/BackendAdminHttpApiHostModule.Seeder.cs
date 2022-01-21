using LY.MicroService.BackendAdmin.DataSeeder;
using Microsoft.Extensions.DependencyInjection;

namespace LY.MicroService.BackendAdmin;

public partial class BackendAdminHttpApiHostModule
{
    private static void ConfigureSeedWorker(IServiceCollection services, bool isDevelopment = false)
    {
        if (isDevelopment)
        {
            services.AddHostedService<BackendAdminDataSeederWorker>();
        }
    }
}
