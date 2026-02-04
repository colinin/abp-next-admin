using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.SecurityLog;

namespace LINGYUN.Abp.AuditLogging;
public interface ISecurityLogQueue
{
    Task EnqueueAsync(SecurityLogInfo securityLogInfo, CancellationToken cancellationToken = default);
}
