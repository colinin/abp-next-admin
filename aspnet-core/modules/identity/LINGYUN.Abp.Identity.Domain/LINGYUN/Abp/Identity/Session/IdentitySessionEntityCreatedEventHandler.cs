using LINGYUN.Abp.Identity.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Identity;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Identity.Session;
public class IdentitySessionEntityCreatedEventHandler :
    ILocalEventHandler<EntityCreatedEventData<IdentitySession>>,
    ITransientDependency
{
    public ILogger<IdentitySessionEntityCreatedEventHandler> Logger { protected get; set; }

    private readonly ISettingProvider _settingProvider;
    private readonly IIdentitySessionStore _identitySessionStore;
    private readonly IDistributedCache<IdentitySessionCacheItem> _identitySessionCache;

    public IdentitySessionEntityCreatedEventHandler(
        ISettingProvider settingProvider,
        IIdentitySessionStore identitySessionStore,
        IDistributedCache<IdentitySessionCacheItem> identitySessionCache)
    {
        _settingProvider = settingProvider;
        _identitySessionStore = identitySessionStore;
        _identitySessionCache = identitySessionCache;

        Logger = NullLogger<IdentitySessionEntityCreatedEventHandler>.Instance;
    }

    public async virtual Task HandleEventAsync(EntityCreatedEventData<IdentitySession> eventData)
    {
        // 创建一个会话后根据策略使其他会话失效
        var strategySet = await _settingProvider.GetOrNullAsync(IdentitySettingNames.Session.ConcurrentLoginStrategy);

        Logger.LogDebug($"The concurrent login strategy is: {strategySet}");

        if (!strategySet.IsNullOrWhiteSpace() && Enum.TryParse<ConcurrentLoginStrategy>(strategySet, true, out var strategy))
        {
            switch (strategy)
            {
                // 限制用户相同设备
                case ConcurrentLoginStrategy.LogoutFromSameTypeDevicesLimit:
                    var sameTypeDevicesCountSet = await _settingProvider.GetAsync(IdentitySettingNames.Session.LogoutFromSameTypeDevicesLimit, 1);
                    Logger.LogDebug($"Clear other sessions on the device {eventData.Entity.Device} and save only {sameTypeDevicesCountSet} sessions.");
                    await _identitySessionStore.RevokeWithAsync(
                        eventData.Entity.UserId, 
                        eventData.Entity.Device,
                        eventData.Entity.Id,
                        sameTypeDevicesCountSet);
                    break;
                // 限制登录设备
                case ConcurrentLoginStrategy.LogoutFromSameTypeDevices:
                    Logger.LogDebug($"Clear all other sessions on the device {eventData.Entity.Device}.");
                    await _identitySessionStore.RevokeAllAsync(
                        eventData.Entity.UserId,
                        eventData.Entity.Device,
                        eventData.Entity.Id);
                    break;
                // 限制多端登录
                case ConcurrentLoginStrategy.LogoutFromAllDevices:
                    Logger.LogDebug($"Clear all other user sessions.");
                    await _identitySessionStore.RevokeAllAsync(
                        eventData.Entity.UserId,
                        eventData.Entity.Id);
                    break;
            }
        }
    }
}
