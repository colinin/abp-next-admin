namespace LINGYUN.Abp.Identity.Session;
public class DeviceInfo
{
    public string Device { get; }
    public string Description { get; }
    public string ClientIpAddress { get; }
    public DeviceInfo(string device, string description, string clientIpAddress)
    {
        Device = device;
        Description = description;
        ClientIpAddress = clientIpAddress;
    }
}
