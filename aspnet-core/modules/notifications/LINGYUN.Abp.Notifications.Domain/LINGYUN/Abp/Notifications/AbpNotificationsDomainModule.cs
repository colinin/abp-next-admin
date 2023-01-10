using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
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
            Configure<NotificationsManagementOptions>(options =>
            {
                options.SaveStaticNotificationsToDatabase = false;
                options.IsDynamicNotificationsStoreEnabled = false;
            });
        }
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        InitializeDynamicNotifications(context);
        return Task.CompletedTask;
    }

    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }

    private void InitializeDynamicNotifications(ApplicationInitializationContext context)
    {
        var options = context
            .ServiceProvider
            .GetRequiredService<IOptions<NotificationsManagementOptions>>()
            .Value;

        if (!options.SaveStaticNotificationsToDatabase && !options.IsDynamicNotificationsStoreEnabled)
        {
            return;
        }

        var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();

        Task.Run(async () =>
        {
            using var scope = rootServiceProvider.CreateScope();
            var applicationLifetime = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
            var cancellationTokenProvider = scope.ServiceProvider.GetRequiredService<ICancellationTokenProvider>();
            var cancellationToken = applicationLifetime?.ApplicationStopping ?? _cancellationTokenSource.Token;

            try
            {
                using (cancellationTokenProvider.Use(cancellationToken))
                {
                    if (cancellationTokenProvider.Token.IsCancellationRequested)
                    {
                        return;
                    }

                    await SaveStaticNotificationsToDatabaseAsync(options, scope, cancellationTokenProvider);

                    if (cancellationTokenProvider.Token.IsCancellationRequested)
                    {
                        return;
                    }

                    await PreCacheDynamicNotificationsAsync(options, scope);
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause (No need to log since it is logged above)
            catch { }
        });
    }

    private async static Task SaveStaticNotificationsToDatabaseAsync(
        NotificationsManagementOptions options,
        IServiceScope scope,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        if (!options.SaveStaticNotificationsToDatabase)
        {
            return;
        }

        await Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(8, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) * 10))
            .ExecuteAsync(async _ =>
            {
                try
                {
                    // ReSharper disable once AccessToDisposedClosure
                    await scope
                        .ServiceProvider
                        .GetRequiredService<IStaticNotificationSaver>()
                        .SaveAsync();
                }
                catch (Exception ex)
                {
                    // ReSharper disable once AccessToDisposedClosure
                    scope.ServiceProvider
                        .GetService<ILogger<AbpNotificationsDomainModule>>()?
                        .LogException(ex);

                    throw; // Polly will catch it
                }
            }, cancellationTokenProvider.Token);
    }

    private async static Task PreCacheDynamicNotificationsAsync(NotificationsManagementOptions options, IServiceScope scope)
    {
        if (!options.IsDynamicNotificationsStoreEnabled)
        {
            return;
        }

        try
        {
            // Pre-cache Notifications, so first request doesn't wait
            await scope
                .ServiceProvider
                .GetRequiredService<IDynamicNotificationDefinitionStore>()
                .GetGroupsAsync();
        }
        catch (Exception ex)
        {
            // ReSharper disable once AccessToDisposedClosure
            scope
                .ServiceProvider
                .GetService<ILogger<AbpNotificationsDomainModule>>()?
                .LogException(ex);

            throw; // It will be cached in InitializeDynamicNotifications
        }
    }
}
