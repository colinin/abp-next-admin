using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.SecurityLog;

namespace LINGYUN.Abp.AuditLogging;
public interface ISecurityLogWriter
{
    Task WriteAsync(SecurityLogInfo securityLogInfo, CancellationToken cancellationToken = default);

    Task BulkWriteAsync(IEnumerable<SecurityLogInfo> securityLogInfos, CancellationToken cancellationToken = default);
}
