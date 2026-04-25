using LINGYUN.Abp.AI.Tools;
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

namespace LINGYUN.Abp.AIManagement.Tools;
public class AIToolDynamicInitializer : ITransientDependency
{
    public ILogger<AIToolDynamicInitializer> Logger { get; set; }

    protected IServiceProvider ServiceProvider { get; }

    public AIToolDynamicInitializer(IServiceProvider serviceProvider)
    {
        Logger = NullLogger<AIToolDynamicInitializer>.Instance;

        ServiceProvider = serviceProvider;
    }

    public virtual Task InitializeAsync(bool runInBackground, CancellationToken cancellationToken = default)
    {
        var options = ServiceProvider.GetRequiredService<IOptions<AIManagementOptions>>().Value;

        if (!options.SaveStaticAIToolsToDatabase && !options.IsDynamicAIToolStoreEnabled)
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

    protected virtual async Task ExecuteInitializationAsync(AIManagementOptions options, CancellationToken cancellationToken)
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

                await SaveStaticAIToolsToDatabaseAsync(options, cancellationToken);

                if (cancellationTokenProvider.Token.IsCancellationRequested)
                {
                    return;
                }

                await PreCacheDynamicAIToolsAsync(options);
            }
        }
        catch
        {
            // No need to log here since inner calls log
        }
    }

    protected virtual async Task SaveStaticAIToolsToDatabaseAsync(
        AIManagementOptions options,
        CancellationToken cancellationToken)
    {
        if (!options.SaveStaticAIToolsToDatabase)
        {
            return;
        }

        var staticAIToolSaver = ServiceProvider.GetRequiredService<IStaticAIToolSaver>();

        await Policy
            .Handle<Exception>(ex => ex is not OperationCanceledException)
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
                    await staticAIToolSaver.SaveAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    throw; // Polly will catch it
                }
            }, cancellationToken);
    }

    protected virtual async Task PreCacheDynamicAIToolsAsync(AIManagementOptions options)
    {
        if (!options.IsDynamicAIToolStoreEnabled)
        {
            return;
        }

        var dynamicAIToolDefinitionStore = ServiceProvider.GetRequiredService<IDynamicAIToolDefinitionStore>();

        try
        {
            // Pre-cache AITools, so first request doesn't wait
            await dynamicAIToolDefinitionStore.GetAllAsync();
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw; // It will be cached in Initialize()
        }
    }
}
