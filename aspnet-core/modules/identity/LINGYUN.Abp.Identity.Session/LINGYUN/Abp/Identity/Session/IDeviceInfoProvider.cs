using System.Threading.Tasks;

namespace LINGYUN.Abp.Identity.Session;
public interface IDeviceInfoProvider
{
    Task<DeviceInfo> GetDeviceInfoAsync();

    string ClientIpAddress { get; }
}
