using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Localization.Persistence;

public class StaticLocalizationSaverHostService : BackgroundService
{
    private readonly AbpLocalizationPersistenceOptions _options;
    private readonly IStaticLocalizationSaver _staticLocalizationSaver;

    public StaticLocalizationSaverHostService(
        IOptions<AbpLocalizationPersistenceOptions> options, 
        IStaticLocalizationSaver staticLocalizationSaver)
    {
        _options = options.Value;
        _staticLocalizationSaver = staticLocalizationSaver;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_options.SaveStaticLocalizationsToPersistence)
        {
            try
            {
                await _staticLocalizationSaver.SaveAsync();
            }
            catch (OperationCanceledException)
            {
                // Ignore
                return;
            }
        }
    }
}
