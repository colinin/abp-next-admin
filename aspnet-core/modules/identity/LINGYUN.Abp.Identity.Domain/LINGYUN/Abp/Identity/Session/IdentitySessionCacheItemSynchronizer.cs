using LINGYUN.Abp.Identity.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.Identity.Session;
public class IdentitySessionCacheItemSynchronizer :
    IDistributedEventHandler<EntityCreatedEto<IdentitySessionEto>>,
    IDistributedEventHandler<EntityDeletedEto<IdentitySessionEto>>,
    IDistributedEventHandler<IdentitySessionChangeAccessedEvent>,
    ILocalEventHandler<EntityDeletedEventData<IdentityUser>>,
    ITransientDependency
{
    public ILogger<IdentitySessionCacheItemSynchronizer> Logger { protected get; set; }
    protected ISettingProvider SettingProvider { get; }
    protected IIdentitySessionCache IdentitySessionCache { get; }
    protected IIdentitySessionStore IdentitySessionStore { get; }

    public IdentitySessionCacheItemSynchronizer(
        ISettingProvider settingProvider,
        IIdentitySessionCache identitySessionCache, 
        IIdentitySessionStore identitySessionStore)
    {
        SettingProvider = settingProvider;
        IdentitySessionCache = identitySessionCache;
        IdentitySessionStore = identitySessionStore;

        Logger = NullLogger<IdentitySessionCacheItemSynchronizer>.Instance;
    }

    public async virtual Task HandleEventAsync(EntityDeletedEto<IdentitySessionEto> eventData)
    {
        await IdentitySessionCache.RemoveAsync(eventData.Entity.SessionId);
    }

    [UnitOfWork]
    public async virtual Task HandleEventAsync(EntityCreatedEto<IdentitySessionEto> eventData)
    {
        await RefreshSessionCache(eventData.Entity);
        await CheckConcurrentLoginStrategy(eventData.Entity);
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

    public async virtual Task HandleEventAsync(EntityDeletedEventData<IdentityUser> eventData)
    {
        // 用户被删除, 移除所有会话
        await IdentitySessionStore.RevokeAllAsync(eventData.Entity.Id);
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

    protected async virtual Task CheckConcurrentLoginStrategy(IdentitySessionEto session)
    {
        // 创建一个会话后根据策略使其他会话失效
        var strategySet = await SettingProvider.GetOrNullAsync(IdentitySettingNames.Session.ConcurrentLoginStrategy);

        Logger.LogDebug($"The concurrent login strategy is: {strategySet}");

        if (!strategySet.IsNullOrWhiteSpace() && Enum.TryParse<ConcurrentLoginStrategy>(strategySet, true, out var strategy))
        {
            switch (strategy)
            {
                // 限制用户相同设备
                case ConcurrentLoginStrategy.LogoutFromSameTypeDevicesLimit:

                    var sameTypeDevicesCountSet = await SettingProvider.GetAsync(IdentitySettingNames.Session.LogoutFromSameTypeDevicesLimit, 1);

                    Logger.LogDebug($"Clear other sessions on the device {session.Device} and save only {sameTypeDevicesCountSet} sessions.");

                    await IdentitySessionStore.RevokeWithAsync(
                        session.UserId,
                        session.Device,
                        session.Id,
                        sameTypeDevicesCountSet);
                    break;
                // 限制登录设备
                case ConcurrentLoginStrategy.LogoutFromSameTypeDevices:

                    Logger.LogDebug($"Clear all other sessions on the device {session.Device}.");

                    await IdentitySessionStore.RevokeAllAsync(
                        session.UserId,
                        session.Device,
                        session.Id);
                    break;
                // 限制多端登录
                case ConcurrentLoginStrategy.LogoutFromAllDevices:

                    Logger.LogDebug($"Clear all other user sessions.");

                    await IdentitySessionStore.RevokeAllAsync(
                        session.UserId,
                        session.Id);
                    break;
            }
        }
    }
}
