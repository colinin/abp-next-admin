using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Identity.Session;

[Dependency(ServiceLifetime.Singleton, TryRegister = true)]
public class NoneIpLocationInfoProvider : IIpLocationInfoProvider
{
    protected static readonly LocationInfo _nullCache = null;
    public Task<LocationInfo> GetLocationInfoAsync(string ipAddress)
    {
        return Task.FromResult(_nullCache);
    }
}
