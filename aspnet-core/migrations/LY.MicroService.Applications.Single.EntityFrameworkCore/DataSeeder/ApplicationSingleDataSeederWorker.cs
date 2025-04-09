using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Data;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.DataSeeder;

public class ApplicationSingleDataSeederWorker : BackgroundService
{
    protected IDataSeeder DataSeeder { get; }

    public ApplicationSingleDataSeederWorker(IDataSeeder dataSeeder)
    {
        DataSeeder = dataSeeder;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await DataSeeder.SeedAsync();
    }
}
