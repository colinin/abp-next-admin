using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.IdentityServer.IdentityResources;

namespace LINGYUN.Abp.IdentityServer.IdentityResources;

[Dependency(ServiceLifetime.Transient)]
[ExposeServices(
    typeof(IIdentityResourceRepository),
    typeof(IdentityResourceRepository),
    typeof(Volo.Abp.IdentityServer.IdentityResources.IIdentityResourceRepository))]
public class EfCoreIdentityResourceRepository : IdentityResourceRepository, IIdentityResourceRepository
{
    public EfCoreIdentityResourceRepository(
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
