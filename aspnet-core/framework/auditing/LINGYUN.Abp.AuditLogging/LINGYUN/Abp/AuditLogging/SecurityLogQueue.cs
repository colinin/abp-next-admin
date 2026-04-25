using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SecurityLog;

namespace LINGYUN.Abp.AuditLogging;
public class SecurityLogQueue : AuditLoggingQueue<SecurityLogInfo>, ISecurityLogQueue, ISingletonDependency
{
    private readonly ISecurityLogWriter _securityLogWriter;
    private readonly ICurrentTenant _currentTenant;
    public SecurityLogQueue(
        IOptions<AbpAuditLoggingOptions> options,
        ISecurityLogWriter securityLogWriter,
        ICurrentTenant currentTenant,
        ILogger<SecurityLogQueue> logger)
        : base(
            "SecurityLog",
            options.Value.MaxSecurityLogQueueSize,
            options.Value.BatchSecurityLogSize,
            logger)
    {
        _securityLogWriter = securityLogWriter;
        _currentTenant = currentTenant;
    }

    protected async override Task BulkWriteAsync(IEnumerable<SecurityLogInfo> securityLogInfos, CancellationToken cancellationToken = default)
    {
        var tenantSecurityLogGroup = securityLogInfos.GroupBy(x => x.TenantId);
        foreach (var tenantSecurityLogs in tenantSecurityLogGroup)
        {
            using (_currentTenant.Change(tenantSecurityLogs.Key))
            {
                await _securityLogWriter.BulkWriteAsync(tenantSecurityLogs, cancellationToken);
            }
        }
    }

    protected async override Task WriteAsync(SecurityLogInfo securityLogInfo, CancellationToken cancellationToken = default)
    {
        using (_currentTenant.Change(securityLogInfo.TenantId))
        {
            await _securityLogWriter.WriteAsync(securityLogInfo, cancellationToken);
        }
    }
}
