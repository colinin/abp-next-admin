using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity.Session;
public class IdentitySessionManager : DomainService, IIdentitySessionManager
{
    protected IDeviceInfoProvider DeviceInfoProvider { get; }
    protected IIdentitySessionStore IdentitySessionStore { get; }
    protected IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache { get; }

    public IdentitySessionManager(
        IDeviceInfoProvider deviceInfoProvider,
        IIdentitySessionStore identitySessionStore,
        IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache)
    {
        DeviceInfoProvider = deviceInfoProvider;
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

            var tenantId = claimsPrincipal.FindTenantId();
            var clientId = claimsPrincipal.FindClientId();

            Logger.LogDebug($"Save user session for user: {userId}, session: {sessionId}");

            await IdentitySessionStore.CreateAsync(
                sessionId,
                device,
                deviceDesc,
                userId.Value,
                clientId,
                clientIpAddress,
                tenantId,
                cancellationToken);

            Logger.LogDebug($"Remove dynamic claims cache for user: {userId}");
            await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(userId.Value, tenantId);
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
