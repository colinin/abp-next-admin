using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch;

public interface IAuditLoggingIndexInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}
