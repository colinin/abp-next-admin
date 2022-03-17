using LINGYUN.Abp.Saas.Editions;
using LINGYUN.Abp.Saas.Tenants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.Saas.EntityFrameworkCore;

public class EfCoreEditionRepository : EfCoreRepository<ISaasDbContext, Edition, Guid>, IEditionRepository
{
    public EfCoreEditionRepository(
        IDbContextProvider<ISaasDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<bool> CheckUsedByTenantAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var tenantDbSet = dbContext.Set<Tenant>();

        return await tenantDbSet
            .AnyAsync(x => x.EditionId == id, GetCancellationToken(cancellationToken));
    }

    public async virtual Task<Edition> FindByDisplayNameAsync(
        string displayName,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(t => t.Id)
            .FirstOrDefaultAsync(t => t.DisplayName == displayName, GetCancellationToken(cancellationToken));
    }

    public async virtual Task<Edition> FindByTenantIdAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var editionDbSet = dbContext.Set<Edition>();
        var tenantDbSet = dbContext.Set<Tenant>();

        var queryable = from tenant in tenantDbSet
                        join edition in editionDbSet
                            on tenant.EditionId equals edition.Id
                        where tenant.Id == tenantId
                        select edition;

        return await queryable
            .OrderBy(t => t.Id)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<long> GetCountAsync(
        string filter = null, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
              .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.DisplayName.Contains(filter))
              .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<Edition>> GetListAsync(
        string sorting = null, 
        int maxResultCount = 10, 
        int skipCount = 0, 
        string filter = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.DisplayName.Contains(filter))
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(Edition.DisplayName) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
