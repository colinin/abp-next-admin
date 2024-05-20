using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.DataProtectionManagement;
public class ProtectedEntitiesSaverService : BackgroundService
{
    private readonly IProtectedEntitiesSaver _protectedEntitiesSaver;

    public ProtectedEntitiesSaverService(IProtectedEntitiesSaver protectedEntitiesSaver)
    {
        _protectedEntitiesSaver = protectedEntitiesSaver;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _protectedEntitiesSaver.SaveAsync(stoppingToken);
    }
}
