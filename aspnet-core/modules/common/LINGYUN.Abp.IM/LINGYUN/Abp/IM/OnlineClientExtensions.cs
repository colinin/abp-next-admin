using JetBrains.Annotations;

namespace LINGYUN.Abp.IM
{
    public static class OnlineClientExtensions
    {
        [CanBeNull]
        public static OnlineClientContext ToClientContextOrNull(this IOnlineClient onlineClient)
        {
            return onlineClient.UserId.HasValue
                ? new OnlineClientContext(onlineClient.ConnectionId, onlineClient.UserId.Value, onlineClient.TenantId)
                : null;
        }
    }
}
