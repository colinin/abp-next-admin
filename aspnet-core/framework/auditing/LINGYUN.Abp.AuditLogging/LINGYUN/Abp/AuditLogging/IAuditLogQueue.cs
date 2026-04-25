using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.AuditLogging;
public interface IAuditLogQueue
{
    Task EnqueueAsync(AuditLogInfo auditLogInfo, CancellationToken cancellationToken = default);
}
