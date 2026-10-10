using System;
using System.Net;

namespace LINGYUN.Abp.IP.Location;
public class IPLocationResolveContext : IIPLocationResolveContext
{
    public IServiceProvider ServiceProvider { get; }

    public IPAddress IpAddress { get; }

    public IPLocation? Location { get; set; }

    public bool Handled { get; set; }

    public bool HasResolvedIPLocation()
    {
        return Handled || Location != null;
    }

    public IPLocationResolveContext(IPAddress ipAddress, IServiceProvider serviceProvider)
    {
        IpAddress = ipAddress;
        ServiceProvider = serviceProvider;
    }
}
