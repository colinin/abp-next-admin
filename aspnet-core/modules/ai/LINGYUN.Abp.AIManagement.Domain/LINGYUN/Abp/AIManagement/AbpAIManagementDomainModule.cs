using LINGYUN.Abp.AI.Agent;
using LINGYUN.Abp.AI.Localization;
using LINGYUN.Abp.AIManagement.Localization;
using LINGYUN.Abp.AIManagement.Workspaces;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Localization;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.AIManagement;

[DependsOn(
    typeof(AbpAIManagementDomainSharedModule),
    typeof(AbpAIAgentModule),
    typeof(AbpCachingModule),
    typeof(AbpMapperlyModule),
    typeof(AbpDddDomainModule))]
public class AbpAIManagementDomainModule : AbpModule
{

    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpAIManagementDomainModule>();

        if (context.Services.IsDataMigrationEnvironment())
        {
            Configure<AIManagementOptions>(options =>
            {
                options.SaveStaticWorkspacesToDatabase = false;
                options.IsDynamicWorkspaceStoreEnabled = false;
            });
        }

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AIManagementResource>()
                .AddBaseTypes(typeof(AbpAIResource));
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();
        var initializer = rootServiceProvider.GetRequiredService<WorkspaceDynamicInitializer>();
        await initializer.InitializeAsync(true, _cancellationTokenSource.Token);
    }

    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }
}
