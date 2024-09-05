using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Identity.Session;

[Dependency(ServiceLifetime.Singleton, TryRegister = true)]
public class NoneDeviceInfoProvider : IDeviceInfoProvider
{
    public static DeviceInfo DeviceInfo => new DeviceInfo("unknown", "unknown", "unknown", "unknown");

    public virtual Task<DeviceInfo> GetDeviceInfoAsync()
    {
        return Task.FromResult(DeviceInfo);
    }

    public string ClientIpAddress => "::1";
}
