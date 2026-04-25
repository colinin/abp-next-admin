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

    public LocalizationDynamicInitializer(IServiceProvider serviceProvider)
    {

        ServiceProvider = serviceProvider;

        Logger = NullLogger<LocalizationDynamicInitializer>.Instance;
    }

    public virtual Task InitializeAsync(bool runInBackground, CancellationToken cancellationToken = default)
    {
        var options = ServiceProvider.GetRequiredService<IOptions<AbpLocalizationManagementOptions>>().Value;

        if (!options.SaveStaticLocalizationsToDatabase)
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

    protected virtual async Task ExecuteInitializationAsync(AbpLocalizationManagementOptions options, CancellationToken cancellationToken)
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

                await SaveStaticLocalizationToDatabaseAsync(options, cancellationToken);

                if (cancellationTokenProvider.Token.IsCancellationRequested)
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

    protected virtual async Task SaveStaticLocalizationToDatabaseAsync(AbpLocalizationManagementOptions options, CancellationToken cancellationToken)
    {
        if (!options.SaveStaticLocalizationsToDatabase)
        {
            return;
        }

        var staticLocalizationSaver = ServiceProvider.GetRequiredService<IStaticLocalizationSaver>();

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

                    await staticLocalizationSaver.SaveAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);

                    throw; // Polly will catch it
                }
            }, cancellationToken);
    }
}
