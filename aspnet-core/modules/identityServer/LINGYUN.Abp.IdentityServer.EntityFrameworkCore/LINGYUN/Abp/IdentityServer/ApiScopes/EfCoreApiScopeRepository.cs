using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

namespace LINGYUN.Abp.IdentityServer.ApiScopes
{
    [Dependency(ServiceLifetime.Transient)]
    [ExposeServices(
        typeof(IApiScopeRepository),
        typeof(ApiScopeRepository),
        typeof(Volo.Abp.IdentityServer.ApiScopes.IApiScopeRepository))]
    public class EfCoreApiScopeRepository : ApiScopeRepository, IApiScopeRepository
    {
        public EfCoreApiScopeRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public virtual async Task<int> GetCountAsync(
            string filter = null, 
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .WhereIf(!filter.IsNullOrWhiteSpace(), x =>
                    x.Name.Contains(filter) || x.DisplayName.Contains(filter) || x.Description.Contains(filter))
                .CountAsync(GetCancellationToken(cancellationToken));
        }
    }
}
