using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Data;

namespace LY.MicroService.IdentityServer.DataSeeder;

public class IdentityServerDataSeederWorker : BackgroundService
{
    protected IDataSeeder DataSeeder { get; }

    public IdentityServerDataSeederWorker(IDataSeeder dataSeeder)
    {
        DataSeeder = dataSeeder;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await DataSeeder.SeedAsync();
    }
}
