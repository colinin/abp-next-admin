using JetBrains.Annotations;

namespace LINGYUN.Abp.RealTime.Client
{
    public static class OnlineClientExtensions
    {
        [CanBeNull]
        public static OnlineClientContext ToClientContextOrNull(this IOnlineClient onlineClient)
        {
            return onlineClient.UserId.HasValue
                ? new OnlineClientContext(onlineClient.TenantId, onlineClient.UserId.Value)
                : null;
        }
    }
}
