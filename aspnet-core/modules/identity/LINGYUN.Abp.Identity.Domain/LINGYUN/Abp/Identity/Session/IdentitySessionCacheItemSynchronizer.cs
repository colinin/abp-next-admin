using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Identity.Session;
public class IdentitySessionCacheItemSynchronizer :
    IDistributedEventHandler<EntityCreatedEto<IdentitySessionEto>>,
    IDistributedEventHandler<EntityDeletedEto<IdentitySessionEto>>,
    ITransientDependency
{
    public ILogger<IdentitySessionCacheItemSynchronizer> Logger { protected get; set; }
    protected ISettingProvider SettingProvider { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected IIdentitySessionCache IdentitySessionCache { get; }

    public IdentitySessionCacheItemSynchronizer(
        ISettingProvider settingProvider,
        IAbpDistributedLock distributedLock,
        IIdentitySessionCache identitySessionCache)
    {
        SettingProvider = settingProvider;
        DistributedLock = distributedLock;
        IdentitySessionCache = identitySessionCache;

        Logger = NullLogger<IdentitySessionCacheItemSynchronizer>.Instance;
    }

    public async virtual Task HandleEventAsync(EntityDeletedEto<IdentitySessionEto> eventData)
    {
        await IdentitySessionCache.RemoveAsync(eventData.Entity.SessionId);
    }

    public async virtual Task HandleEventAsync(EntityCreatedEto<IdentitySessionEto> eventData)
    {
        var lockKey = $"{nameof(IdentitySessionCacheItemSynchronizer)}_{nameof(EntityCreatedEto<IdentitySessionEto>)}";
        await using (var handle = await DistributedLock.TryAcquireAsync(lockKey))
        {
            Logger.LogInformation($"Lock is acquired for {lockKey}");

            if (handle == null)
            {
                Logger.LogInformation($"Handle is null because of the locking for : {lockKey}");
                return;
            }

            await RefreshSessionCache(eventData.Entity);
        }
    }

    protected async virtual Task RefreshSessionCache(IdentitySessionEto session)
    {
        var identitySessionCacheItem = new IdentitySessionCacheItem(
            session.Device,
            session.DeviceInfo,
            session.UserId,
            session.SessionId,
            session.ClientId,
            session.IpAddresses,
            session.SignedIn,
            session.LastAccessed);

        await IdentitySessionCache.RefreshAsync(
            session.SessionId,
            identitySessionCacheItem);
    }
}
