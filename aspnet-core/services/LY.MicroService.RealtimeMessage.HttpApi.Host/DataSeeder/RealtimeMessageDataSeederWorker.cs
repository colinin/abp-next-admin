using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Data;

namespace LY.MicroService.RealtimeMessage.DataSeeder;

public class RealtimeMessageDataSeederWorker : BackgroundService
{
    protected IDataSeeder DataSeeder { get; }

    public RealtimeMessageDataSeederWorker(IDataSeeder dataSeeder)
    {
        DataSeeder = dataSeeder;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await DataSeeder.SeedAsync();
    }
}
