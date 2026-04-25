using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AuditLogging;

[Dependency(TryRegister = true)]
public class LoggerAuditLogWriter : IAuditLogWriter, ISingletonDependency
{
    private readonly ILogger<LoggerAuditLogWriter> _logger;
    public LoggerAuditLogWriter(ILogger<LoggerAuditLogWriter> logger)
    {
        _logger = logger;
    }

    public async virtual Task BulkWriteAsync(IEnumerable<AuditLogInfo> auditLogInfos, CancellationToken cancellationToken = default)
    {
        foreach (var auditLogInfo in auditLogInfos)
        {
            await WriteAsync(auditLogInfo, cancellationToken);
        }
    }

    public virtual Task WriteAsync(AuditLogInfo auditLogInfo, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(auditLogInfo.ToString());

        return Task.CompletedTask;
    }
}
