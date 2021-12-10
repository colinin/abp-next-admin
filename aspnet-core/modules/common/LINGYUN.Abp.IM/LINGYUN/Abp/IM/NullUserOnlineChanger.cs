using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.IM
{
    [Dependency(TryRegister = true)]
    public class NullUserOnlineChanger : IUserOnlineChanger, ISingletonDependency
    {
        public Task ChangeAsync(Guid? tenantId, Guid userId, UserOnlineState state, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
