using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.IM
{
    [Dependency(TryRegister = true)]
    public class NullUserOnlineChecker : IUserOnlineChecker, ISingletonDependency
    {
        public Task<bool> CheckAsync(Guid? tenantId, Guid userId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }
    }
}
