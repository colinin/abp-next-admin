using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Platform.Portal;
public interface IEnterpriseRepository : IRepository<Enterprise, Guid>
{
    Task<Enterprise> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<List<Enterprise>> GetEnterprisesInTenantListAsync(
        int maxResultCount = 10,
        CancellationToken cancellationToken = default);

    Task<Guid?> GetEnterpriseInTenantAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
