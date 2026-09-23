using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.AuditLogging;
using Volo.Abp.Specifications;

using VoloAuditLog = Volo.Abp.AuditLogging.AuditLog;

namespace LINGYUN.Abp.AuditLogging.EntityFrameworkCore;

public interface IEfCoreAuditLogRepository : IAuditLogRepository
{
    Task<long> GetCountAsync(
        ISpecification<VoloAuditLog> specification,
        CancellationToken cancellationToken = default);

    Task<List<VoloAuditLog>> GetListAsync(
        ISpecification<VoloAuditLog> specification,
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);
}
