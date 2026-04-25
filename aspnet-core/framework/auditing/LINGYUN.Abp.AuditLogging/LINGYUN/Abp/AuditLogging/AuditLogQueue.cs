using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.AuditLogging;
public class AuditLogQueue : AuditLoggingQueue<AuditLogInfo>, IAuditLogQueue, ISingletonDependency
{
    private readonly IAuditLogWriter _auditLogWriter;
    private readonly ICurrentTenant _currentTenant;
    public AuditLogQueue(
        IOptions<AbpAuditLoggingOptions> options,
        IAuditLogWriter auditLogWriter,
        ICurrentTenant currentTenant,
        ILogger<AuditLogQueue> logger)
        : base(
            "AuditLog",
            options.Value.MaxAuditLogQueueSize,
            options.Value.BatchAuditLogSize,
            logger)
    {
        _auditLogWriter = auditLogWriter;
        _currentTenant = currentTenant;
    }

    protected async override Task BulkWriteAsync(IEnumerable<AuditLogInfo> auditLogInfos, CancellationToken cancellationToken = default)
    {
        var tenantAuditlogGroup = auditLogInfos.GroupBy(x => x.ImpersonatorTenantId ?? x.TenantId);
        foreach (var tenantAuditlogs in tenantAuditlogGroup)
        {
            using (_currentTenant.Change(tenantAuditlogs.Key))
            {
                await _auditLogWriter.BulkWriteAsync(tenantAuditlogs, cancellationToken);
            }
        }
    }

    protected async override Task WriteAsync(AuditLogInfo auditLogInfo, CancellationToken cancellationToken = default)
    {
        using (_currentTenant.Change(auditLogInfo.ImpersonatorTenantId ?? auditLogInfo.TenantId))
        {
            await _auditLogWriter.WriteAsync(auditLogInfo, cancellationToken);
        }
    }
}
