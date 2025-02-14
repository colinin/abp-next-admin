using System;

namespace LINGYUN.Abp.IP.Location;
public class IPLocationResolveContext : IIPLocationResolveContext
{
    public IServiceProvider ServiceProvider { get; }

    public string IpAddress { get; }

    public IPLocation? Location { get; set; }

    public bool Handled { get; set; }

    public bool HasResolvedIPLocation()
    {
        return Handled || Location != null;
    }

    public IPLocationResolveContext(string ipAddress, IServiceProvider serviceProvider)
    {
        IpAddress = ipAddress;
        ServiceProvider = serviceProvider;
    }
}
