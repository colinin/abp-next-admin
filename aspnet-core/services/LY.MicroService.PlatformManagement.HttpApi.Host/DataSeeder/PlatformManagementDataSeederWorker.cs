using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Data;

namespace LY.MicroService.PlatformManagement.DataSeeder;

public class PlatformManagementDataSeederWorker : BackgroundService
{
    protected IDataSeeder DataSeeder { get; }

    public PlatformManagementDataSeederWorker(IDataSeeder dataSeeder)
    {
        DataSeeder = dataSeeder;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await DataSeeder.SeedAsync();
    }
}
