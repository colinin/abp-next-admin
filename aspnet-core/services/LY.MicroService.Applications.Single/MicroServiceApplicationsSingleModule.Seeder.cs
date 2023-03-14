using LY.MicroService.Applications.Single.DataSeeder;

namespace LY.MicroService.Applications.Single;

public partial class MicroServiceApplicationsSingleModule
{
    private static void ConfigureSeedWorker(IServiceCollection services, bool isDevelopment = false)
    {
        services.AddHostedService<DataSeederWorker>();

        if (isDevelopment)
        {
            services.AddHostedService<DataSeederWorker>();
        }
    }
}
