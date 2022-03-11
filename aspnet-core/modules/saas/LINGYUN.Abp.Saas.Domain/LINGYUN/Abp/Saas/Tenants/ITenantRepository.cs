
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.Saas.Tenants;

public interface ITenantRepository : IBasicRepository<Tenant, Guid>
{
    [Obsolete("Use FindByNameAsync method.")]
    Tenant FindByName(
        string name,
        bool includeDetails = true
    );

    [Obsolete("Use FindAsync method.")]
    Tenant FindById(
        Guid id,
        bool includeDetails = true
    );

    Task<Tenant> FindByNameAsync(
       string name,
       bool includeDetails = true,
       CancellationToken cancellationToken = default);

    Task<List<Tenant>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        string filter = null,
        CancellationToken cancellationToken = default);
}
