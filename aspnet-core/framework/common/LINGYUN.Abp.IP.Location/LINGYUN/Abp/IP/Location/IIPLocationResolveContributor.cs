using System.Threading.Tasks;

namespace LINGYUN.Abp.IP.Location;
public interface IIPLocationResolveContributor
{
    string Name { get; }

    Task ResolveAsync(IIPLocationResolveContext context);
}
