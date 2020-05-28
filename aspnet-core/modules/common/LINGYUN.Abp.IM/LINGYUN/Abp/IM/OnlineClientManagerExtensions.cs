using JetBrains.Annotations;
using System.Linq;
using Volo.Abp;

namespace LINGYUN.Abp.IM
{
    public static class OnlineClientManagerExtensions
    {
        public static bool IsOnline(
            [NotNull] this IOnlineClientManager onlineClientManager,
            [NotNull] OnlineClientContext context)
        {
            return onlineClientManager.GetAllByContext(context).Any();
        }

        public static bool Remove(
            [NotNull] this IOnlineClientManager onlineClientManager,
            [NotNull] IOnlineClient client)
        {
            Check.NotNull(onlineClientManager, nameof(onlineClientManager));
            Check.NotNull(client, nameof(client));

            return onlineClientManager.Remove(client.ConnectionId);
        }
    }
}
