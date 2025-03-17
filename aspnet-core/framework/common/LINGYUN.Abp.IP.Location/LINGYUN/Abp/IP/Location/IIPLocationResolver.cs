using JetBrains.Annotations;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IP.Location;
public interface IIPLocationResolver
{
    [NotNull]
    Task<IPLocationResolveResult> ResolveAsync(string ipAddress);
}
