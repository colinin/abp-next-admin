using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IdentityServer.ApiScopes
{
    public interface IApiScopeRepository : Volo.Abp.IdentityServer.ApiScopes.IApiScopeRepository
    {
        Task<int> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default
        );
    }
}
