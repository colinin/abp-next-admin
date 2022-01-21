using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Data;

namespace LY.MicroService.BackendAdmin.DataSeeder;

public class BackendAdminDataSeederWorker : BackgroundService
{
    protected IDataSeeder DataSeeder { get; }

    public BackendAdminDataSeederWorker(IDataSeeder dataSeeder)
    {
        DataSeeder = dataSeeder;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await DataSeeder.SeedAsync();
    }
}
