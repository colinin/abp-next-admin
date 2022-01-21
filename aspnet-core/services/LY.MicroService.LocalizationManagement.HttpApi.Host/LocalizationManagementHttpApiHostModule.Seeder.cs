using LY.MicroService.LocalizationManagement.DataSeeder;
using Microsoft.Extensions.DependencyInjection;

namespace LY.MicroService.LocalizationManagement;

public partial class LocalizationManagementHttpApiHostModule
{
    private static void ConfigureSeedWorker(IServiceCollection services, bool isDevelopment = false)
    {
        if (isDevelopment)
        {
            services.AddHostedService<LocalizationManagementDataSeederWorker>();
        }
    }
}
