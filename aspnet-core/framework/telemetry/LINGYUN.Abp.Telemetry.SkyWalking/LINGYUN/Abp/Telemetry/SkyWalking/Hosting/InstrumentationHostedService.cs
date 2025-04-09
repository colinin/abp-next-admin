using Microsoft.Extensions.Hosting;
using SkyApm;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Telemetry.SkyWalking.Hosting;

internal class InstrumentationHostedService : IHostedService
{
    private readonly IInstrumentStartup _startup;

    public InstrumentationHostedService(IInstrumentStartup startup)
    {
        _startup = startup;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return _startup.StartAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _startup.StopAsync(cancellationToken);
    }
}
