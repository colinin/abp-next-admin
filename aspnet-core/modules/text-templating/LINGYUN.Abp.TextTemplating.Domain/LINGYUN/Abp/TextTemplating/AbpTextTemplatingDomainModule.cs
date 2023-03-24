using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.TextTemplating;

[DependsOn(
    typeof(AbpTextTemplatingDomainSharedModule),
    typeof(AbpTextTemplatingCoreModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpCachingModule))]
public class AbpTextTemplatingDomainModule : AbpModule
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpTextTemplatingDomainModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<TextTemplateMapperProfile>(validate: true);
        });

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<TextTemplate, TextTemplateEto>();
            options.EtoMappings.Add<TextTemplateDefinition, TextTemplateDefinitionEto>();
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

    public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        return context.ServiceProvider
            .GetRequiredService<TextTemplateDefinitionInitializer>()
            .InitializeDynamicTemplates(_cancellationTokenSource.Token);
    }

    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }
}
