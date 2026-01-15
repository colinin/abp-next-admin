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
    protected IOptions<AbpNotificationsManagementOptions> Options { get; }
    protected IHostApplicationLifetime ApplicationLifetime { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected IDynamicNotificationDefinitionStore DynamicNotificationDefinitionStore { get; }
    protected IStaticNotificationSaver StaticNotificationSaver { get; }

    public NotificationDynamicInitializer(
        IServiceProvider serviceProvider,
        IOptions<AbpNotificationsManagementOptions> options,
        ICancellationTokenProvider cancellationTokenProvider,
        IDynamicNotificationDefinitionStore dynamicNotificationDefinitionStore,
        IStaticNotificationSaver staticNotificationSaver)
    {
        ServiceProvider = serviceProvider;
        Options = options;
        CancellationTokenProvider = cancellationTokenProvider;
        DynamicNotificationDefinitionStore = dynamicNotificationDefinitionStore;
        StaticNotificationSaver = staticNotificationSaver;
        ApplicationLifetime = ServiceProvider.GetService<IHostApplicationLifetime>();

        Logger = NullLogger<NotificationDynamicInitializer>.Instance;
    }

    public virtual Task InitializeAsync(bool runInBackground, CancellationToken cancellationToken = default)
    {
        if (!Options.Value.SaveStaticNotificationsToDatabase && !Options.Value.IsDynamicNotificationsStoreEnabled)
        {
            return Task.CompletedTask;
        }

        if (runInBackground)
        {
            Task.Run(async () =>
            {
                if (cancellationToken == default && ApplicationLifetime?.ApplicationStopping != null)
                {
                    cancellationToken = ApplicationLifetime.ApplicationStopping;
                }
                await ExecuteInitializationAsync(cancellationToken);
            }, cancellationToken);
            return Task.CompletedTask;
        }

        return ExecuteInitializationAsync(cancellationToken);
    }

    protected virtual async Task ExecuteInitializationAsync(CancellationToken cancellationToken)
    {
        try
        {
            using (CancellationTokenProvider.Use(cancellationToken))
            {
                if (CancellationTokenProvider.Token.IsCancellationRequested)
                {
                    return;
                }

                await SaveStaticNotificationssToDatabaseAsync(cancellationToken);

                if (CancellationTokenProvider.Token.IsCancellationRequested)
                {
                    return;
                }

                await PreCacheDynamicNotificationsAsync(cancellationToken);
            }
        }
        catch
        {
            // No need to log here since inner calls log
        }
    }

    protected virtual async Task SaveStaticNotificationssToDatabaseAsync(CancellationToken cancellationToken)
    {
        if (!Options.Value.SaveStaticNotificationsToDatabase)
        {
            return;
        }

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

                    await StaticNotificationSaver.SaveAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);

                    throw; // Polly will catch it
                }
            }, cancellationToken);
    }

    protected virtual async Task PreCacheDynamicNotificationsAsync(CancellationToken cancellationToken)
    {
        if (!Options.Value.IsDynamicNotificationsStoreEnabled)
        {
            return;
        }

        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            // Pre-cache notifications, so first request doesn't wait
            await DynamicNotificationDefinitionStore.GetGroupsAsync();
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);

            throw; // It will be cached in Initialize()
        }
    }
}
