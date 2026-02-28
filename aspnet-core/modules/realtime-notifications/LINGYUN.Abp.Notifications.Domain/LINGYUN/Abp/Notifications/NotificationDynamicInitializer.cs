using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Polly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.Notifications;
public class NotificationDynamicInitializer : ITransientDependency
{
    public ILogger<NotificationDynamicInitializer> Logger { get; set; }
    protected IServiceProvider ServiceProvider { get; }

    public NotificationDynamicInitializer(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;

        Logger = NullLogger<NotificationDynamicInitializer>.Instance;
    }

    public virtual Task InitializeAsync(bool runInBackground, CancellationToken cancellationToken = default)
    {
        var options = ServiceProvider.GetRequiredService<IOptions<AbpNotificationsManagementOptions>>().Value;

        if (!options.SaveStaticNotificationsToDatabase && !options.IsDynamicNotificationsStoreEnabled)
        {
            return Task.CompletedTask;
        }

        if (runInBackground)
        {
            var applicationLifetime = ServiceProvider.GetService<IHostApplicationLifetime>();
            Task.Run(async () =>
            {
                if (cancellationToken == default && applicationLifetime?.ApplicationStopping != null)
                {
                    cancellationToken = applicationLifetime.ApplicationStopping;
                }
                await ExecuteInitializationAsync(options, cancellationToken);
            }, cancellationToken);
            return Task.CompletedTask;
        }

        return ExecuteInitializationAsync(options, cancellationToken);
    }

    protected virtual async Task ExecuteInitializationAsync(AbpNotificationsManagementOptions options, CancellationToken cancellationToken)
    {
        try
        {
            var cancellationTokenProvider = ServiceProvider.GetRequiredService<ICancellationTokenProvider>();
            using (cancellationTokenProvider.Use(cancellationToken))
            {
                if (cancellationTokenProvider.Token.IsCancellationRequested)
                {
                    return;
                }

                await SaveStaticNotificationssToDatabaseAsync(options, cancellationToken);

                if (cancellationTokenProvider.Token.IsCancellationRequested)
                {
                    return;
                }

                await PreCacheDynamicNotificationsAsync(options, cancellationToken);
            }
        }
        catch
        {
            // No need to log here since inner calls log
        }
    }

    protected virtual async Task SaveStaticNotificationssToDatabaseAsync(AbpNotificationsManagementOptions options, CancellationToken cancellationToken)
    {
        if (!options.SaveStaticNotificationsToDatabase)
        {
            return;
        }

        var staticNotificationSaver = ServiceProvider.GetRequiredService<IStaticNotificationSaver>();

        await Policy
            .Handle<Exception>(e => e is not OperationCanceledException)
            .WaitAndRetryAsync(
                8,
                retryAttempt => TimeSpan.FromSeconds(
                    Volo.Abp.RandomHelper.GetRandom(
                        (int)Math.Pow(2, retryAttempt) * 8,
                        (int)Math.Pow(2, retryAttempt) * 12)
                )
            )
            .ExecuteAsync(async _ =>
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    await staticNotificationSaver.SaveAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);

                    throw; // Polly will catch it
                }
            }, cancellationToken);
    }

    protected virtual async Task PreCacheDynamicNotificationsAsync(AbpNotificationsManagementOptions options, CancellationToken cancellationToken)
    {
        if (!options.IsDynamicNotificationsStoreEnabled)
        {
            return;
        }

        var dynamicNotificationDefinitionStore = ServiceProvider.GetRequiredService<IDynamicNotificationDefinitionStore>();

        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            // Pre-cache notifications, so first request doesn't wait
            await dynamicNotificationDefinitionStore.GetGroupsAsync();
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);

            throw; // It will be cached in Initialize()
        }
    }
}
