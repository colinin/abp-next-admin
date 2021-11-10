using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IM
{
    public interface IUserOnlineChanger
    {
        Task ChangeAsync(
            Guid? tenantId,
            Guid userId,
            UserOnlineState state,
            CancellationToken cancellationToken = default);
    }
}
