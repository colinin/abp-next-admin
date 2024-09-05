namespace LINGYUN.Abp.Identity.Session;
public class DeviceInfo
{
    public string Device { get; }
    public string Description { get; }
    public string ClientIpAddress { get; }
    public string IpRegion { get; }
    public DeviceInfo(string device, string description, string clientIpAddress, string ipRegion)
    {
        Device = device;
        Description = description;
        ClientIpAddress = clientIpAddress;
        IpRegion = ipRegion;
    }
}
