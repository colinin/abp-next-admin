using Volo.Abp.Data;

namespace LY.MicroService.Applications.Single.DataSeeder;

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
