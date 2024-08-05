using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

namespace LINGYUN.Abp.IdentityServer.ApiResources;

[Dependency(ServiceLifetime.Transient)]
[ExposeServices(
    typeof(IApiResourceRepository),
    typeof(ApiResourceRepository),
    typeof(Volo.Abp.IdentityServer.ApiResources.IApiResourceRepository))]
public class EfCoreApiResourceRepository : ApiResourceRepository, IApiResourceRepository
{
    public EfCoreApiResourceRepository(
        IDbContextProvider<IIdentityServerDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<List<string>> GetNamesAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Select(x => x.Name)
            .Distinct()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
