using LINGYUN.Abp.Identity;
using LINGYUN.Abp.Identity.Session;
using LINGYUN.Abp.Identity.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Settings;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace LY.MicroService.AuthServer.Handlers;
/// <summary>
/// 会话控制事件处理器
/// </summary>
public class IdentitySessionAccessEventHandler :
    IDistributedEventHandler<IdentitySessionChangeAccessedEvent>,
    IDistributedEventHandler<EntityCreatedEto<IdentitySessionEto>>,
    IDistributedEventHandler<EntityDeletedEto<UserEto>>,
    ITransientDependency
{
    public ILogger<IdentitySessionAccessEventHandler> Logger { protected get; set; }
    protected ISettingProvider SettingProvider { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected IIdentitySessionCache IdentitySessionCache { get; }
    protected IIdentitySessionStore IdentitySessionStore { get; }

    public IdentitySessionAccessEventHandler(
        ISettingProvider settingProvider,
        IAbpDistributedLock distributedLock,
        IIdentitySessionCache identitySessionCache,
        IIdentitySessionStore identitySessionStore)
    {
        SettingProvider = settingProvider;
        DistributedLock = distributedLock;
        IdentitySessionCache = identitySessionCache;
        IdentitySessionStore = identitySessionStore;

        Logger = NullLogger<IdentitySessionAccessEventHandler>.Instance;
    }

    [UnitOfWork]
    public async virtual Task HandleEventAsync(EntityCreatedEto<IdentitySessionEto> eventData)
    {
        // 新会话创建时检查登录策略
        var lockKey = $"{nameof(IdentitySessionAccessEventHandler)}_{nameof(EntityCreatedEto<IdentitySessionEto>)}";
        await using (var handle = await DistributedLock.TryAcquireAsync(lockKey))
        {
            Logger.LogInformation($"Lock is acquired for {lockKey}");

            if (handle == null)
            {
                Logger.LogInformation($"Handle is null because of the locking for : {lockKey}");
                return;
            }

            await CheckConcurrentLoginStrategy(eventData.Entity);
        }
    }

    [UnitOfWork]
    public async virtual Task HandleEventAsync(EntityDeletedEto<UserEto> eventData)
    {
        // 用户被删除, 移除所有会话
        var lockKey = $"{nameof(IdentitySessionAccessEventHandler)}_{nameof(EntityDeletedEto<UserEto>)}";
        await using (var handle = await DistributedLock.TryAcquireAsync(lockKey))
        {
            Logger.LogInformation($"Lock is acquired for {lockKey}");

            if (handle == null)
            {
                Logger.LogInformation($"Handle is null because of the locking for : {lockKey}");
                return;
            }

            await IdentitySessionStore.RevokeAllAsync(eventData.Entity.Id);
        }
    }

    [UnitOfWork]
    public async virtual Task HandleEventAsync(IdentitySessionChangeAccessedEvent eventData)
    {
        // 会话访问更新
        var lockKey = $"{nameof(IdentitySessionAccessEventHandler)}_{nameof(IdentitySessionChangeAccessedEvent)}";
        await using (var handle = await DistributedLock.TryAcquireAsync(lockKey))
        {
            Logger.LogInformation($"Lock is acquired for {lockKey}");

            if (handle == null)
            {
                Logger.LogInformation($"Handle is null because of the locking for : {lockKey}");
                return;
            }

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
