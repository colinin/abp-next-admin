using System.Net;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.IP.Location;
public interface IIPLocationResolveContext : IServiceProviderAccessor
{
    IPAddress IpAddress { get; }

    IPLocation? Location { get; set; }

    bool Handled { get; set; }
}
