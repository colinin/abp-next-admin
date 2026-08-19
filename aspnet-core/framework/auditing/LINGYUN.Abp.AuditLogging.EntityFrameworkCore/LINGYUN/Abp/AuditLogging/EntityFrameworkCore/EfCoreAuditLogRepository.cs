using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.AuditLogging;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

using VoloAuditLog = Volo.Abp.AuditLogging.AuditLog;

namespace LINGYUN.Abp.AuditLogging.EntityFrameworkCore;

public class EfCoreAuditLogRepository : Volo.Abp.AuditLogging.EntityFrameworkCore.EfCoreAuditLogRepository, IEfCoreAuditLogRepository
{
    public EfCoreAuditLogRepository(
        IDbContextProvider<IAuditLoggingDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async virtual Task<long> GetCountAsync(
        ISpecification<VoloAuditLog> specification, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<VoloAuditLog>> GetListAsync(
        ISpecification<VoloAuditLog> specification, 
        string? sorting = null, 
        int maxResultCount = 50,
        int skipCount = 0, 
        bool includeDetails = false, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .IncludeDetails(includeDetails)
            .Where(specification.ToExpression())
            .OrderBy(sorting.IsNullOrWhiteSpace() ? $"{nameof(VoloAuditLog.ExecutionTime)} DESC" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
