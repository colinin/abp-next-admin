using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Localization;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.LocalizationManagement;

[DependsOn(
    typeof(AbpMapperlyModule),
    typeof(AbpDddDomainModule),
    typeof(AbpLocalizationManagementDomainSharedModule))]
public class AbpLocalizationManagementDomainModule : AbpModule
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpLocalizationManagementDomainModule>();

        if (context.Services.IsDataMigrationEnvironment())
        {
            Configure<AbpLocalizationManagementOptions>(options =>
            {
                options.SaveStaticLocalizationsToDatabase = false;
            });
        }

        Configure<AbpLocalizationOptions>(options =>
        {
            options.GlobalContributors.Add<LocalizationResourceContributor>();
        });

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<Text, TextEto>(typeof(AbpLocalizationManagementDomainModule));
            options.EtoMappings.Add<Language, LanguageEto>(typeof(AbpLocalizationManagementDomainModule));
            options.EtoMappings.Add<Resource, ResourceEto>(typeof(AbpLocalizationManagementDomainModule));
        });

        // 定期更新本地化缓存缓解措施
        context.Services.AddHostedService<LocalizationTextCacheRefreshWorker>();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();
        var initializer = rootServiceProvider.GetRequiredService<LocalizationDynamicInitializer>();
        await initializer.InitializeAsync(true, _cancellationTokenSource.Token);
    }

    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        _cancellationTokenSource.CancelAsync();
        return Task.CompletedTask;
    }
}
