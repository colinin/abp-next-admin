using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MicroService.AIService;
public class AIServiceDataSeeder : ITransientDependency
{
    protected ILogger<AIServiceDataSeeder> Logger { get; }
    protected ICurrentTenant CurrentTenant { get; }

    public AIServiceDataSeeder(
        ICurrentTenant currentTenant)
    {
        CurrentTenant = currentTenant;

        Logger = NullLogger<AIServiceDataSeeder>.Instance;
    }

    public virtual Task SeedAsync(DataSeedContext context)
    {
        return Task.CompletedTask;
    }
}
