using LY.MicroService.AuthServer.DataSeeder;
using Microsoft.Extensions.DependencyInjection;

namespace LY.MicroService.AuthServer;

public partial class AuthServerModule
{
    private static void ConfigureSeedWorker(IServiceCollection services, bool isDevelopment = false)
    {
        if (isDevelopment)
        {
            services.AddHostedService<AuthServerDataSeederWorker>();
        }
    }
}
