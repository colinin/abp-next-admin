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

    public WebhookDynamicInitializer(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;

        Logger = NullLogger<WebhookDynamicInitializer>.Instance;
    }

    public virtual Task InitializeAsync(bool runInBackground, CancellationToken cancellationToken = default)
    {
        var options = ServiceProvider.GetRequiredService<IOptions<WebhooksManagementOptions>>().Value;

        if (!options.SaveStaticWebhooksToDatabase && !options.IsDynamicWebhookStoreEnabled)
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

    protected virtual async Task ExecuteInitializationAsync(WebhooksManagementOptions options, CancellationToken cancellationToken)
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

                await SaveStaticWebhooksToDatabaseAsync(options, cancellationToken);

                if (cancellationTokenProvider.Token.IsCancellationRequested)
                {
                    return;
                }

                await PreCacheDynamicWebhooksAsync(options, cancellationToken);
            }
        }
        catch
        {
            // No need to log here since inner calls log
        }
    }

    protected virtual async Task SaveStaticWebhooksToDatabaseAsync(WebhooksManagementOptions options, CancellationToken cancellationToken)
    {
        if (!options.SaveStaticWebhooksToDatabase)
        {
            return;
        }

        var staticWebhookSaver = ServiceProvider.GetRequiredService<IStaticWebhookSaver>();

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

                    await staticWebhookSaver.SaveAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);

                    throw; // Polly will catch it
                }
            }, cancellationToken);
    }

    protected virtual async Task PreCacheDynamicWebhooksAsync(WebhooksManagementOptions options, CancellationToken cancellationToken)
    {
        if (!options.IsDynamicWebhookStoreEnabled)
        {
            return;
        }

        var dynamicWebhookDefinitionStore = ServiceProvider.GetRequiredService<IDynamicWebhookDefinitionStore>();

        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Pre-cache webhoks, so first request doesn't wait
            await dynamicWebhookDefinitionStore.GetGroupsAsync();
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);

            throw; // It will be cached in Initialize()
        }
    }
}
