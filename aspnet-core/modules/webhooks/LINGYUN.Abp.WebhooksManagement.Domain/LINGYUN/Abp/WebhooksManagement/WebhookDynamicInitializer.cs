using JetBrains.Annotations;
using LINGYUN.Abp.Webhooks;
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

namespace LINGYUN.Abp.WebhooksManagement;
public class WebhookDynamicInitializer : ITransientDependency
{
    public ILogger<WebhookDynamicInitializer> Logger { get; set; }

    protected IServiceProvider ServiceProvider { get; }
    protected IOptions<WebhooksManagementOptions> Options { get; }
    [CanBeNull]
    protected IHostApplicationLifetime ApplicationLifetime { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected IDynamicWebhookDefinitionStore DynamicWebhookDefinitionStore { get; }
    protected IStaticWebhookSaver StaticWebhookSaver { get; }

    public WebhookDynamicInitializer(
        IServiceProvider serviceProvider,
        IOptions<WebhooksManagementOptions> options,
        ICancellationTokenProvider cancellationTokenProvider,
        IDynamicWebhookDefinitionStore dynamicWebhookDefinitionStore,
        IStaticWebhookSaver staticWebhookSaver)
    {
        ServiceProvider = serviceProvider;
        Options = options;
        ApplicationLifetime = ServiceProvider.GetService<IHostApplicationLifetime>();
        CancellationTokenProvider = cancellationTokenProvider;
        DynamicWebhookDefinitionStore = dynamicWebhookDefinitionStore;
        StaticWebhookSaver = staticWebhookSaver;

        Logger = NullLogger<WebhookDynamicInitializer>.Instance;
    }

    public virtual Task InitializeAsync(bool runInBackground, CancellationToken cancellationToken = default)
    {
        if (!Options.Value.SaveStaticWebhooksToDatabase && !Options.Value.IsDynamicWebhookStoreEnabled)
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

                await SaveStaticWebhooksToDatabaseAsync(cancellationToken);

                if (CancellationTokenProvider.Token.IsCancellationRequested)
                {
                    return;
                }

                await PreCacheDynamicWebhooksAsync(cancellationToken);
            }
        }
        catch
        {
            // No need to log here since inner calls log
        }
    }

    protected virtual async Task SaveStaticWebhooksToDatabaseAsync(CancellationToken cancellationToken)
    {
        if (!Options.Value.SaveStaticWebhooksToDatabase)
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

                    await StaticWebhookSaver.SaveAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);

                    throw; // Polly will catch it
                }
            }, cancellationToken);
    }

    protected virtual async Task PreCacheDynamicWebhooksAsync(CancellationToken cancellationToken)
    {
        if (!Options.Value.IsDynamicWebhookStoreEnabled)
        {
            return;
        }

        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Pre-cache webhoks, so first request doesn't wait
            await DynamicWebhookDefinitionStore.GetGroupsAsync();
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);

            throw; // It will be cached in Initialize()
        }
    }
}
