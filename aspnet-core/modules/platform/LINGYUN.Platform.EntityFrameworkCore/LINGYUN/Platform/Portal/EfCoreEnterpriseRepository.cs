using LINGYUN.Platform.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Platform.Portal;
public class EfCoreEnterpriseRepository : EfCoreRepository<IPlatformDbContext, Enterprise, Guid>, IEnterpriseRepository
{
    public EfCoreEnterpriseRepository(
        IDbContextProvider<IPlatformDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<Guid?> GetEnterpriseInTenantAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.Id == id)
            .Select(x => x.TenantId)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<Enterprise>> GetEnterprisesInTenantListAsync(
        int maxResultCount = 10, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.TenantId.HasValue)
            .Take(maxResultCount)
            .OrderByDescending(x => x.CreationTime)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
