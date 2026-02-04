using LINGYUN.Abp.Elasticsearch;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch;

[DependsOn(
    typeof(AbpAuditLoggingModule),
    typeof(AbpElasticsearchModule),
    typeof(AbpJsonModule))]
public class AbpAuditLoggingElasticsearchModule : AbpModule
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<AbpAuditLoggingElasticsearchOptions>(configuration.GetSection("AuditLogging:Elasticsearch"));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();
        var initializer = rootServiceProvider.GetRequiredService<IAuditLoggingIndexInitializer>();
        await initializer.InitializeAsync(_cancellationTokenSource.Token);
    }
}
