namespace LY.MicroService.Applications.Single.DataSeeder;

public class DataSeederWorker : BackgroundService
{
    protected IDataSeeder DataSeeder { get; }

    public DataSeederWorker(IDataSeeder dataSeeder)
    {
        DataSeeder = dataSeeder;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await DataSeeder.SeedAsync();
    }
}
