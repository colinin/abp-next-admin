using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AuditLogging
{
    [Dependency(ReplaceServices = true)]
    public class AuditingStore : IAuditingStore, ITransientDependency
    {
        private readonly IAuditLogManager _manager;

        public AuditingStore(
            IAuditLogManager manager)
        {
            _manager = manager;
        }

        public virtual async Task SaveAsync(AuditLogInfo auditInfo)
        {
            await _manager.SaveAsync(auditInfo);
        }
    }
}
