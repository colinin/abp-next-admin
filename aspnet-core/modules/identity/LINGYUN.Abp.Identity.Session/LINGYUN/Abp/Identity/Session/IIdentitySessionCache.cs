using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Identity.Session;
public interface IIdentitySessionCache
{
    Task RefreshAsync(string sessionId, IdentitySessionCacheItem cacheItem, CancellationToken cancellationToken = default);

    Task<IdentitySessionCacheItem> GetAsync(string sessionId, CancellationToken cancellationToken = default);

    Task RemoveAsync(string sessionId, CancellationToken cancellationToken = default);
}
