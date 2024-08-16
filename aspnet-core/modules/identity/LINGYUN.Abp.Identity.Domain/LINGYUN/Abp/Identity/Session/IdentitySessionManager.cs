using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.Identity.Session;
public class IdentitySessionManager : DomainService, IIdentitySessionManager
{
    protected IDeviceInfoProvider DeviceInfoProvider { get; }
    protected IIdentitySessionCache IdentitySessionCache { get; }
    protected IIdentitySessionStore IdentitySessionStore { get; }
    protected IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache { get; }

    public IdentitySessionManager(
        IDeviceInfoProvider deviceInfoProvider,
        IIdentitySessionCache identitySessionCache,
        IIdentitySessionStore identitySessionStore,
        IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache)
    {
        DeviceInfoProvider = deviceInfoProvider;
        IdentitySessionCache = identitySessionCache;
        IdentitySessionStore = identitySessionStore;
        IdentityDynamicClaimsPrincipalContributorCache = identityDynamicClaimsPrincipalContributorCache;
    }

    [DisableAuditing]
    public async virtual Task SaveSessionAsync(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken = default)
    {
        if (claimsPrincipal != null)
        {
            var userId = claimsPrincipal.FindUserId();
            var tenantId = claimsPrincipal.FindTenantId();

            using (CurrentTenant.Change(tenantId))
            {
                var sessionId = claimsPrincipal.FindSessionId();
                if (!userId.HasValue || sessionId.IsNullOrWhiteSpace())
                {
                    return;
                }
                if (await IdentitySessionStore.ExistAsync(sessionId, cancellationToken))
                {
                    return;
                }
                var deviceInfo = DeviceInfoProvider.DeviceInfo;

                var device = deviceInfo.Device ?? IdentitySessionDevices.OAuth;
                var deviceDesc = deviceInfo.Description;
                var clientIpAddress = deviceInfo.ClientIpAddress;

                var clientId = claimsPrincipal.FindClientId();

                Logger.LogDebug($"Save user session for user: {userId}, session: {sessionId}");

                await IdentitySessionStore.CreateAsync(
                    sessionId,
                    device,
                    deviceDesc,
                    userId.Value,
                    clientId,
                    clientIpAddress,
                    Clock.Now,
                    Clock.Now,
                    tenantId,
                    cancellationToken);

                Logger.LogDebug($"Remove dynamic claims cache for user: {userId}");

                await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(userId.Value, tenantId);

                await IdentitySessionCache.RefreshAsync(sessionId,
                    new IdentitySessionCacheItem(
                        device,
                        deviceDesc,
                        userId.Value,
                        sessionId,
                        clientId,
                        clientIpAddress,
                        Clock.Now,
                        Clock.Now));
            }
        }
    }

    public async virtual Task RevokeSessionAsync(
        string sessionId,
        CancellationToken cancellation = default)
    {
        Logger.LogDebug($"Revoke user session for: {sessionId}");
        await IdentitySessionStore.RevokeAsync(sessionId, cancellation);
    }
}
