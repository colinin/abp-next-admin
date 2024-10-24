using System;

namespace LINGYUN.Abp.Identity.Session;

[Serializable]
public class IdentitySessionCacheItem
{
    private const string CacheKeyFormat = "s:{0}";
    /// <summary>
    /// 登录设备
    /// </summary>
    public string Device { get; set; }
    /// <summary>
    /// 设备描述
    /// </summary>
    public string DeviceInfo { get; set; }
    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// 会话Id
    /// </summary>
    public string SessionId { get; set; }
    /// <summary>
    /// 客户端Id
    /// </summary>
    public string ClientId { get; set; }
    /// <summary>
    /// IP地址
    /// </summary>
    public string IpAddresses { get; set; }
    /// <summary>
    /// IP属地
    /// </summary>
    public string IpRegion { get; set; }
    /// <summary>
    /// 登录时间
    /// </summary>
    public DateTime SignedIn { get; set; }
    /// <summary>
    /// 上次访问时间
    /// </summary>
    public DateTime? LastAccessed { get; set; }
    /// <summary>
    /// 过期时间(ms)
    /// </summary>
    public double? ExpiraIn { get; set; }

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
        DateTime? lastAccessed = null,
        string ipRegion = null,
        double? expiraIn = null)
    {
        Device = device;
        DeviceInfo = deviceInfo;
        UserId = userId;
        SessionId = sessionId;
        ClientId = clientId;
        IpAddresses = ipAddresses;
        IpRegion = ipRegion;
        SignedIn = signedIn;
        LastAccessed = lastAccessed;
        ExpiraIn = expiraIn;
    }

    public static string CalculateCacheKey(string sessionId)
    {
        return string.Format(CacheKeyFormat, sessionId);
    }
}