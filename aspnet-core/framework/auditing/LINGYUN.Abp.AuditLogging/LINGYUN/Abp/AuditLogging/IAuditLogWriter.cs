using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.AuditLogging;
public interface IAuditLogWriter
{
    Task WriteAsync(AuditLogInfo auditLogInfo, CancellationToken cancellationToken = default);

    Task BulkWriteAsync(IEnumerable<AuditLogInfo> auditLogInfos, CancellationToken cancellationToken = default);
}
