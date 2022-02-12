using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SecurityLog;

namespace LINGYUN.Abp.AuditLogging
{
    [Dependency(ReplaceServices = true)]
    public class SecurityLogStore : ISecurityLogStore, ITransientDependency
    {
        private readonly ISecurityLogManager _manager;

        public SecurityLogStore(
            ISecurityLogManager manager)
        {
            _manager = manager;
        }

        public virtual async Task SaveAsync(SecurityLogInfo securityLogInfo)
        {
            await _manager.SaveAsync(securityLogInfo);
        }
    }
}
