using LINGYUN.Abp.AI.Workspaces;
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

namespace LINGYUN.Abp.AIManagement.Workspaces;
public class WorkspaceDynamicInitializer : ITransientDependency
{
    public ILogger<WorkspaceDynamicInitializer> Logger { get; set; }

    protected IServiceProvider ServiceProvider { get; }

    public WorkspaceDynamicInitializer(IServiceProvider serviceProvider)
    {
        Logger = NullLogger<WorkspaceDynamicInitializer>.Instance;

        ServiceProvider = serviceProvider;
    }

    public virtual Task InitializeAsync(bool runInBackground, CancellationToken cancellationToken = default)
    {
        var options = ServiceProvider.GetRequiredService<IOptions<AIManagementOptions>>().Value;

        if (!options.SaveStaticWorkspacesToDatabase && !options.IsDynamicWorkspaceStoreEnabled)
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

                await SaveStaticWorkspacesToDatabaseAsync(options, cancellationToken);

                if (cancellationTokenProvider.Token.IsCancellationRequested)
                {
                    return;
                }

                await PreCacheDynamicWorkspacesAsync(options);
            }
        }
        catch
        {
            // No need to log here since inner calls log
        }
    }

    protected virtual async Task SaveStaticWorkspacesToDatabaseAsync(
        AIManagementOptions options,
        CancellationToken cancellationToken)
    {
        if (!options.SaveStaticWorkspacesToDatabase)
        {
            return;
        }

        var staticWorkspaceSaver = ServiceProvider.GetRequiredService<IStaticWorkspaceSaver>();

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
                    await staticWorkspaceSaver.SaveAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    throw; // Polly will catch it
                }
            }, cancellationToken);
    }

    protected virtual async Task PreCacheDynamicWorkspacesAsync(AIManagementOptions options)
    {
        if (!options.IsDynamicWorkspaceStoreEnabled)
        {
            return;
        }

        var dynamicWorkspaceDefinitionStore = ServiceProvider.GetRequiredService<IDynamicWorkspaceDefinitionStore>();

        try
        {
            // Pre-cache Workspaces, so first request doesn't wait
            await dynamicWorkspaceDefinitionStore.GetAllAsync();
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw; // It will be cached in Initialize()
        }
    }
}
