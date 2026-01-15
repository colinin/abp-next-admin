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

namespace LINGYUN.Abp.TextTemplating;
public class TextTemplateDynamicInitializer : ITransientDependency
{
    public ILogger<TextTemplateDynamicInitializer> Logger { get; set; }

    protected IServiceProvider ServiceProvider { get; }
    protected IOptions<AbpTextTemplatingCachingOptions> Options { get; }
    protected IHostApplicationLifetime ApplicationLifetime { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected ITemplateDefinitionStore TemplateDefinitionStore { get; }
    protected IStaticTemplateSaver StaticTemplateSaver { get; }

    public TextTemplateDynamicInitializer(
        IServiceProvider serviceProvider,
        IOptions<AbpTextTemplatingCachingOptions> options,
        ICancellationTokenProvider cancellationTokenProvider,
        ITemplateDefinitionStore templateDefinitionStore,
        IStaticTemplateSaver staticTemplateSaver)
    {
        ServiceProvider = serviceProvider;
        Options = options;
        ApplicationLifetime = ServiceProvider.GetService<IHostApplicationLifetime>();
        CancellationTokenProvider = cancellationTokenProvider;
        TemplateDefinitionStore = templateDefinitionStore;
        StaticTemplateSaver = staticTemplateSaver;

        Logger = NullLogger<TextTemplateDynamicInitializer>.Instance;
    }

    public virtual Task InitializeAsync(bool runInBackground, CancellationToken cancellationToken = default)
    {
        if (!Options.Value.SaveStaticTemplateDefinitionToDatabase && !Options.Value.IsDynamicTemplateDefinitionStoreEnabled)
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

                await SaveStaticTextTemplatesToDatabaseAsync(cancellationToken);

                if (CancellationTokenProvider.Token.IsCancellationRequested)
                {
                    return;
                }

                await PreCacheDynamicTextTemplatesAsync(cancellationToken);
            }
        }
        catch
        {
            // No need to log here since inner calls log
        }
    }

    protected virtual async Task SaveStaticTextTemplatesToDatabaseAsync(CancellationToken cancellationToken)
    {
        if (!Options.Value.SaveStaticTemplateDefinitionToDatabase)
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

                    await StaticTemplateSaver.SaveDefinitionTemplateAsync();
                    await StaticTemplateSaver.SaveTemplateContentAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);

                    throw; // Polly will catch it
                }
            }, cancellationToken);
    }

    protected virtual async Task PreCacheDynamicTextTemplatesAsync(CancellationToken cancellationToken)
    {
        if (!Options.Value.IsDynamicTemplateDefinitionStoreEnabled)
        {
            return;
        }

        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Pre-cache tempte definitions, so first request doesn't wait
            await TemplateDefinitionStore.GetAllAsync();
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);

            throw; // It will be cached in Initialize()
        }
    }
}
