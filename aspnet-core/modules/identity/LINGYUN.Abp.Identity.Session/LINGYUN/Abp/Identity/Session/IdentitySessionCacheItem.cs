using System;

namespace LINGYUN.Abp.Identity.Session;

[Serializable]
public class IdentitySessionCacheItem
{
    private const string CacheKeyFormat = "s:{0}";

    public string Device { get; set; }

    public string DeviceInfo { get; set; }

    public Guid UserId { get; set; }

    public string SessionId { get; set; }

    public string ClientId { get; set; }

    public string IpAddresses { get; set; }

    public DateTime SignedIn { get; set; }

    public DateTime? LastAccessed { get; set; }

    public IdentitySessionCacheItem()
    {
    }

    public IdentitySessionCacheItem(
        string device,
        string deviceInfo,
        Guid userId,
        string sessionId,
        string clientId,
        string ipAddresses,
        DateTime signedIn,
        DateTime? lastAccessed = null)
    {
        Device = device;
        DeviceInfo = deviceInfo;
        UserId = userId;
        SessionId = sessionId;
        ClientId = clientId;
        IpAddresses = ipAddresses;
        SignedIn = signedIn;
        LastAccessed = lastAccessed;
    }

    public static string CalculateCacheKey(string sessionId)
    {
        return string.Format(CacheKeyFormat, sessionId);
    }
}