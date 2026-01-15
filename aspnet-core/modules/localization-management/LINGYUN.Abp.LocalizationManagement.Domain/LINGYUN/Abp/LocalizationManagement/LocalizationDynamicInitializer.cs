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

namespace LINGYUN.Abp.LocalizationManagement;
public class LocalizationDynamicInitializer : ITransientDependency
{
    public ILogger<LocalizationDynamicInitializer> Logger { get; set; }

    protected IServiceProvider ServiceProvider { get; }
    protected IOptions<AbpLocalizationManagementOptions> Options { get; }
    protected IHostApplicationLifetime ApplicationLifetime { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected IStaticLocalizationSaver StaticLocalizationSaver { get; }

    public LocalizationDynamicInitializer(
        IServiceProvider serviceProvider,
        IOptions<AbpLocalizationManagementOptions> options,
        ICancellationTokenProvider cancellationTokenProvider,
        IStaticLocalizationSaver staticLocalizationSaver)
    {

        ServiceProvider = serviceProvider;
        Options = options;
        ApplicationLifetime = ServiceProvider.GetService<IHostApplicationLifetime>();
        CancellationTokenProvider = cancellationTokenProvider;
        StaticLocalizationSaver = staticLocalizationSaver;

        Logger = NullLogger<LocalizationDynamicInitializer>.Instance;
    }

    public virtual Task InitializeAsync(bool runInBackground, CancellationToken cancellationToken = default)
    {
        if (!Options.Value.SaveStaticLocalizationsToDatabase)
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

                await SaveStaticLocalizationToDatabaseAsync(cancellationToken);

                if (CancellationTokenProvider.Token.IsCancellationRequested)
                {
                    return;
                }
            }
        }
        catch
        {
            // No need to log here since inner calls log
        }
    }

    protected virtual async Task SaveStaticLocalizationToDatabaseAsync(CancellationToken cancellationToken)
    {
        if (!Options.Value.SaveStaticLocalizationsToDatabase)
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

                    await StaticLocalizationSaver.SaveAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);

                    throw; // Polly will catch it
                }
            }, cancellationToken);
    }
}
