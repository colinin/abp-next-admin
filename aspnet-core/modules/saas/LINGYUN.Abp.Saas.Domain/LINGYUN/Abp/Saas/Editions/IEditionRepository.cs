using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.Saas.Editions;

public interface IEditionRepository : IBasicRepository<Edition, Guid>
{
    Task<bool> CheckUsedByTenantAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Edition> FindByDisplayNameAsync(
        string displayName,
        CancellationToken cancellationToken = default);

    Task<Edition> FindByTenantIdAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default);

    Task<List<Edition>> GetListAsync(
        string sorting = null,
        int maxResultCount = 10,
        int skipCount = 0,
        string filter = null,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        string filter = null,
        CancellationToken cancellationToken = default);
}
