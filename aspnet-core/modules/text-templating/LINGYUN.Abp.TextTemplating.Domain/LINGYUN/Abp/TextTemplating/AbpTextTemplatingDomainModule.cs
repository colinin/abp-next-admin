using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.TextTemplating;

[DependsOn(
    typeof(AbpTextTemplatingDomainSharedModule),
    typeof(AbpTextTemplatingCoreModule),
    typeof(AbpMapperlyModule),
    typeof(AbpCachingModule))]
public class AbpTextTemplatingDomainModule : AbpModule
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpTextTemplatingDomainModule>();

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<TextTemplate, TextTemplateEto>(typeof(AbpTextTemplatingDomainModule));
            options.EtoMappings.Add<TextTemplateDefinition, TextTemplateDefinitionEto>(typeof(AbpTextTemplatingDomainModule));

            // TODO: CAP组件异常将导致应用无法启动, 临时禁用
            // options.AutoEventSelectors.Add<TextTemplate>();
        });

        if (context.Services.IsDataMigrationEnvironment())
        {
            Configure<AbpTextTemplatingCachingOptions>(options =>
            {
                options.SaveStaticTemplateDefinitionToDatabase = false;
                options.IsDynamicTemplateDefinitionStoreEnabled = false;
            });
        }
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();
        var initializer = rootServiceProvider.GetRequiredService<TextTemplateDynamicInitializer>();
        await initializer.InitializeAsync(true, _cancellationTokenSource.Token);
    }

    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }
}
