using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Data;

namespace LY.MicroService.WorkflowManagement.DataSeeder;

public class WorkflowManagementDataSeederWorker : BackgroundService
{
    protected IDataSeeder DataSeeder { get; }

    public WorkflowManagementDataSeederWorker(IDataSeeder dataSeeder)
    {
        DataSeeder = dataSeeder;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await DataSeeder.SeedAsync();
    }
}
