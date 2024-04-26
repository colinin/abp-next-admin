using LINGYUN.Abp.Webhooks;
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

namespace LINGYUN.Abp.WebhooksManagement;
public class WebhookDefinitionInitializer : ITransientDependency
{
    protected IRootServiceProvider RootServiceProvider { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected WebhooksManagementOptions WebhooksManagementOptions { get; }
    public WebhookDefinitionInitializer(
        IRootServiceProvider rootServiceProvider,
        ICancellationTokenProvider cancellationTokenProvider,
        IOptions<WebhooksManagementOptions> webhooksManagementOptions)
    {
        RootServiceProvider = rootServiceProvider;
        CancellationTokenProvider = cancellationTokenProvider;
        WebhooksManagementOptions = webhooksManagementOptions.Value;
    }

    [UnitOfWork]
    public async virtual Task InitializeDynamicWebhooks(CancellationToken cancellationToken)
    {
        if (!WebhooksManagementOptions.SaveStaticWebhooksToDatabase && !WebhooksManagementOptions.IsDynamicWebhookStoreEnabled)
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

                await PreCacheDynamicNotificationsAsync(scope);
            }
        }
        catch (OperationCanceledException)
        {
            // ignore
        }
        catch (Exception ex)
        {
            scope.ServiceProvider
                .GetService<ILogger<WebhookDefinitionInitializer>>()?
                .LogException(ex);
        }
    }

    private async Task SaveStaticNotificationsToDatabaseAsync(IServiceScope serviceScope)
    {
        if (!WebhooksManagementOptions.SaveStaticWebhooksToDatabase)
        {
            return;
        }

        var saver = serviceScope.ServiceProvider.GetRequiredService<IStaticWebhookSaver>();

        await saver.SaveAsync();
    }

    private async Task PreCacheDynamicNotificationsAsync(IServiceScope serviceScope)
    {
        if (!WebhooksManagementOptions.IsDynamicWebhookStoreEnabled)
        {
            return;
        }

        var store = serviceScope.ServiceProvider.GetRequiredService<IDynamicWebhookDefinitionStore>();

        await store.GetGroupsAsync();
    }
}
