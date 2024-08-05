using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.Identity.Session;

public class DefaultIdentitySessionChecker : IIdentitySessionChecker, ITransientDependency
{
    public ILogger<DefaultIdentitySessionChecker> Logger { protected get; set; }

    protected IClock Clock { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IDeviceInfoProvider DeviceInfoProvider { get; }
    protected IDistributedEventBus DistributedEventBus { get; }
    protected IIdentitySessionCache IdentitySessionCache { get; }
    protected IdentitySessionCheckOptions SessionCheckOptions { get; }

    public DefaultIdentitySessionChecker(
        IClock clock, 
        ICurrentTenant currentTenant, 
        IDeviceInfoProvider deviceInfoProvider,
        IDistributedEventBus distributedEventBus,
        IIdentitySessionCache identitySessionCache,
        IOptions<IdentitySessionCheckOptions> sessionCheckOptions)
    {
        Clock = clock;
        CurrentTenant = currentTenant;
        DeviceInfoProvider = deviceInfoProvider;
        DistributedEventBus = distributedEventBus;
        IdentitySessionCache = identitySessionCache;
        SessionCheckOptions = sessionCheckOptions.Value;

        Logger = NullLogger<DefaultIdentitySessionChecker>.Instance;
    }

    public async virtual Task<bool> ValidateSessionAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        if (sessionId.IsNullOrWhiteSpace())
        {
            Logger.LogDebug("No user session id found.");
            return false;
        }

        var identitySessionCacheItem = await IdentitySessionCache.GetAsync(sessionId, cancellationToken);
        if (identitySessionCacheItem == null)
        {
            Logger.LogDebug($"No user session cache found for: {sessionId}.");
            return false;
        }

        // Implementation https://github.com/abpio/abp-commercial-docs/blob/dev/en/modules/identity/session-management.md#how-it-works

        var lastAccressedTime = identitySessionCacheItem.LastAccessed;
        var accressedTime = Clock.Now;

        if (lastAccressedTime.HasValue &&
            lastAccressedTime.Value < accressedTime.Subtract(SessionCheckOptions.KeepAccessTimeSpan))
        {
            // 更新缓存中的访问地址以及客户端Ip地址
            identitySessionCacheItem.LastAccessed = accressedTime;
            identitySessionCacheItem.IpAddresses = DeviceInfoProvider.ClientIpAddress;

            Logger.LogDebug($"Refresh the user access info in the cache from {sessionId}.");
            await IdentitySessionCache.RefreshAsync(sessionId, identitySessionCacheItem, cancellationToken);
        }

        // 避免某些场景频繁去刷新持久化设施
        if (lastAccressedTime.HasValue &&
            lastAccressedTime.Value < accressedTime.Subtract(SessionCheckOptions.SessionSyncTimeSpan))
        {
            Logger.LogDebug($"Publishes the cache synchronization user session event from {sessionId}.");
            // 发布事件, 使持久化设施从缓存同步
            var eventData = new IdentitySessionChangeAccessedEvent(
                identitySessionCacheItem.SessionId,
                identitySessionCacheItem.IpAddresses,
                accressedTime,
                CurrentTenant.Id);

            await DistributedEventBus.PublishAsync(eventData);
        }

        return true;
    }
}
