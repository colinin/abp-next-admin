namespace LINGYUN.Abp.Identity.Session;
public interface IDeviceInfoProvider
{
    DeviceInfo DeviceInfo { get; }

    string ClientIpAddress { get; }
}
