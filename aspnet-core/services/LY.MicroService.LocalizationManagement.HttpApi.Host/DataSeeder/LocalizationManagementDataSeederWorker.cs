using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Data;

namespace LY.MicroService.LocalizationManagement.DataSeeder;

public class LocalizationManagementDataSeederWorker : BackgroundService
{
    protected IDataSeeder DataSeeder { get; }

    public LocalizationManagementDataSeederWorker(IDataSeeder dataSeeder)
    {
        DataSeeder = dataSeeder;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await DataSeeder.SeedAsync();
    }
}
