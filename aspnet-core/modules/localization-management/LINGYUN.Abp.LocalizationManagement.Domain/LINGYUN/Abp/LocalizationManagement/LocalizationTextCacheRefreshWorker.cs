using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Localization;
using Volo.Abp.Localization.External;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.LocalizationManagement;
/// <summary>
/// 本地化缓存刷新作业
/// </summary>
public class LocalizationTextCacheRefreshWorker : BackgroundService
{
    private readonly AbpAsyncTimer _timer;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ILogger<LocalizationTextCacheRefreshWorker> Logger { protected get; set; }
    protected CancellationToken StoppingToken { get; set; }

    public LocalizationTextCacheRefreshWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _timer = timer;
        _timer.Period = 60000;
        _timer.Elapsed += Timer_Elapsed;

        Logger = NullLogger<LocalizationTextCacheRefreshWorker>.Instance;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        StoppingToken = stoppingToken;
        _timer.Start(stoppingToken);

        return Task.CompletedTask;
    }

    private async Task Timer_Elapsed(AbpAsyncTimer timer)
    {
        await DoWorkAsync(StoppingToken);
    }

    public async virtual Task DoWorkAsync(CancellationToken cancellationToken = default)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            try
            {
                // 定期刷新本地化缓存
                var cache = scope.ServiceProvider.GetService<LocalizationTextStoreCache>();
                if (cache != null)
                {
                    var options = scope.ServiceProvider.GetRequiredService<IOptions<AbpLocalizationOptions>>().Value;
                    var languageProvider = scope.ServiceProvider.GetRequiredService<ILanguageProvider>();
                    var externalLocalizationStore = scope.ServiceProvider.GetRequiredService<IExternalLocalizationStore>();

                    var languages = await languageProvider.GetLanguagesAsync();

                    var resources = options
                        .Resources
                        .Values
                        .Union(
                            await externalLocalizationStore.GetResourcesAsync()
                        ).ToArray();


                    foreach (var language in languages)
                    {
                        foreach (var resource in resources)
                        {
                            await cache.UpdateStaticCache(resource, language.CultureName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await scope.ServiceProvider
                    .GetRequiredService<IExceptionNotifier>()
                    .NotifyAsync(new ExceptionNotificationContext(ex));

                Logger.LogException(ex);
            }
        }
    }
}
