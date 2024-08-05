using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Identity.Session;

[Dependency(ServiceLifetime.Singleton, TryRegister = true)]
public class NoneDeviceInfoProvider : IDeviceInfoProvider
{
    public DeviceInfo DeviceInfo => new DeviceInfo("unknown", "unknown", "unknown");

    public string ClientIpAddress => "::1";
}
