using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Data;

namespace LY.MicroService.WebhooksManagement.DataSeeder;

public class WebhooksManagementDataSeederWorker : BackgroundService
{
    protected IDataSeeder DataSeeder { get; }

    public WebhooksManagementDataSeederWorker(IDataSeeder dataSeeder)
    {
        DataSeeder = dataSeeder;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await DataSeeder.SeedAsync();
    }
}
