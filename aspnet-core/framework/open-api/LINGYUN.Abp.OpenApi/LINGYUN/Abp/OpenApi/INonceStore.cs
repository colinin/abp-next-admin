using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OpenApi;
public interface INonceStore
{
    Task<bool> TrySetAsync(string nonce, CancellationToken cancellationToken = default);
}
