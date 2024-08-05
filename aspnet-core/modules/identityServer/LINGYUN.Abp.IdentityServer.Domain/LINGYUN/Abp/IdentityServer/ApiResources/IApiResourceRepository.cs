using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IdentityServer.ApiResources;

public interface IApiResourceRepository : Volo.Abp.IdentityServer.ApiResources.IApiResourceRepository
{
    Task<List<string>> GetNamesAsync(CancellationToken cancellationToken = default);
}
