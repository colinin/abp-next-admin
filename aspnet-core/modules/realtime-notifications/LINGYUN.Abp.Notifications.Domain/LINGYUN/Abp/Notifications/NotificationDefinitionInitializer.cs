using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.Notifications;
public class NotificationDefinitionInitializer : ITransientDependency
{
    protected IRootServiceProvider RootServiceProvider { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected AbpNotificationsManagementOptions NotificationsManagementOptions { get; }
    public NotificationDefinitionInitializer(
        IRootServiceProvider rootServiceProvider,
        ICancellationTokenProvider cancellationTokenProvider,
        IOptions<AbpNotificationsManagementOptions> notificationsManagementOptions)
    {
        RootServiceProvider = rootServiceProvider;
        CancellationTokenProvider = cancellationTokenProvider;
        NotificationsManagementOptions = notificationsManagementOptions.Value;
    }

    [UnitOfWork]
    public async virtual Task InitializeDynamicNotifications(CancellationToken cancellationToken)
    {
        if (!NotificationsManagementOptions.SaveStaticNotificationsToDatabase && !NotificationsManagementOptions.IsDynamicNotificationsStoreEnabled)
        {
            return;
        }

        using var scope = RootServiceProvider.CreateScope();
        var applicationLifetime = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
        var token = applicationLifetime?.ApplicationStopping ?? cancellationToken;
        try
        {
            using (CancellationTokenProvider.Use(cancellationToken))
            {
                if (CancellationTokenProvider.Token.IsCancellationRequested)
                {
                    return;
                }

                await SaveStaticNotificationsToDatabaseAsync(scope);

                if (CancellationTokenProvider.Token.IsCancellationRequested)
                {
                    return;
                }

                // await PreCacheDynamicNotificationsAsync(scope);
            }
        }
        catch (OperationCanceledException)
        {
            // ignore
        }
        catch(Exception ex)
        {
            scope.ServiceProvider
                .GetService<ILogger<NotificationDefinitionInitializer>>()?
                .LogException(ex);
        }
    }

    private async Task SaveStaticNotificationsToDatabaseAsync(IServiceScope serviceScope)
    {
        if (!NotificationsManagementOptions.SaveStaticNotificationsToDatabase)
        {
            return;
        }

        var saver = serviceScope.ServiceProvider.GetRequiredService<IStaticNotificationSaver>();

        await saver.SaveAsync();
    }

    private async Task PreCacheDynamicNotificationsAsync(IServiceScope serviceScope)
    {
        if (!NotificationsManagementOptions.IsDynamicNotificationsStoreEnabled)
        {
            return;
        }

        var store = serviceScope.ServiceProvider.GetRequiredService<IDynamicNotificationDefinitionStore>();

        await store.GetGroupsAsync();
    }
}
