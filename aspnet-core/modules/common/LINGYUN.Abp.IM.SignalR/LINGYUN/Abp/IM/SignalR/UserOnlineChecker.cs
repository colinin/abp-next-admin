using LINGYUN.Abp.RealTime.Client;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.IM.SignalR
{
    public class UserOnlineChecker : IUserOnlineChecker, ITransientDependency
    {
        private readonly IOnlineClientManager _onlineClientManager;

        public UserOnlineChecker(
            IOnlineClientManager onlineClientManager)
        {
            _onlineClientManager = onlineClientManager;
        }

        public virtual Task<bool> CheckAsync(
            Guid? tenantId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var onlineClients = _onlineClientManager
                .GetAllClients(client => client.UserId.Equals(userId));

            var userOnlined = onlineClients?.Any() == true;

            return Task.FromResult(userOnlined);
        }
    }
}
