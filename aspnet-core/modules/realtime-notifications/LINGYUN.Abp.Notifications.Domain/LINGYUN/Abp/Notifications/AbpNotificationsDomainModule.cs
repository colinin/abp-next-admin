using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.Notifications;

[DependsOn(
    typeof(AbpCachingModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpNotificationsModule),
    typeof(AbpNotificationsDomainSharedModule))]
public class AbpNotificationsDomainModule : AbpModule
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        if (context.Services.IsDataMigrationEnvironment())
        {
            Configure<AbpNotificationsManagementOptions>(options =>
            {
                options.SaveStaticNotificationsToDatabase = false;
                options.IsDynamicNotificationsStoreEnabled = false;
            });
        }

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpNotificationsDomainAutoMapperProfile>(validate: true);
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        return context.ServiceProvider
            .GetRequiredService<NotificationDefinitionInitializer>()
            .InitializeDynamicNotifications(_cancellationTokenSource.Token);
    }

    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }
}
