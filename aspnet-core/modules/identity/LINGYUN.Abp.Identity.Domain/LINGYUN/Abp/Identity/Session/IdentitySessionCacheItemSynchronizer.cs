using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.Identity.Session;
public class IdentitySessionCacheItemSynchronizer :
    IDistributedEventHandler<EntityCreatedEto<IdentitySessionEto>>,
    IDistributedEventHandler<EntityDeletedEto<IdentitySessionEto>>,
    IDistributedEventHandler<IdentitySessionChangeAccessedEvent>,
    ITransientDependency
{
    protected IIdentitySessionCache IdentitySessionCache { get; }
    protected IIdentitySessionStore IdentitySessionStore { get; }

    public IdentitySessionCacheItemSynchronizer(
        IIdentitySessionCache identitySessionCache, 
        IIdentitySessionStore identitySessionStore)
    {
        IdentitySessionCache = identitySessionCache;
        IdentitySessionStore = identitySessionStore;
    }

    public async virtual Task HandleEventAsync(EntityDeletedEto<IdentitySessionEto> eventData)
    {
        await IdentitySessionCache.RemoveAsync(eventData.Entity.SessionId);
    }

    public async virtual Task HandleEventAsync(EntityCreatedEto<IdentitySessionEto> eventData)
    {
        var identitySessionCacheItem = new IdentitySessionCacheItem(
            eventData.Entity.Device,
            eventData.Entity.DeviceInfo,
            eventData.Entity.UserId,
            eventData.Entity.SessionId,
            eventData.Entity.ClientId,
            eventData.Entity.IpAddresses,
            eventData.Entity.SignedIn,
            eventData.Entity.LastAccessed);

        await IdentitySessionCache.RefreshAsync(
            eventData.Entity.SessionId,
            identitySessionCacheItem);
    }

    public async virtual Task HandleEventAsync(IdentitySessionChangeAccessedEvent eventData)
    {
        var idetitySession = await IdentitySessionStore.FindAsync(eventData.SessionId);
        if (idetitySession != null)
        {
            if (!eventData.IpAddresses.IsNullOrWhiteSpace())
            {
                idetitySession.SetIpAddresses(eventData.IpAddresses.Split(","));
            }
            idetitySession.UpdateLastAccessedTime(eventData.LastAccessed);

            await IdentitySessionStore.UpdateAsync(idetitySession);
        }
        else
        {
            // 数据库中不存在会话, 清理缓存, 后续请求会话失效
            await IdentitySessionCache.RemoveAsync(eventData.SessionId);
        }
    }
}
