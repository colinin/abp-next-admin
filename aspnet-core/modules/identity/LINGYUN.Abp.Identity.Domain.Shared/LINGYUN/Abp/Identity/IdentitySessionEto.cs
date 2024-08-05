using System;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Identity;

[Serializable]
public class IdentitySessionEto : IMultiTenant
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public string SessionId { get; set; }

    public string Device { get; set; }

    public string DeviceInfo { get; set; }

    public Guid UserId { get; set; }

    public string ClientId { get; set; }

    public string IpAddresses { get; set; }

    public DateTime SignedIn { get; set; }

    public DateTime? LastAccessed { get; set; }
    public IdentitySessionEto()
    {

    }
    public IdentitySessionEto(
        Guid id, 
        string sessionId,
        string device, 
        string deviceInfo,
        Guid userId, 
        string clientId,
        string ipAddresses, 
        DateTime signedIn, 
        DateTime? lastAccessed,
        Guid? tenantId = null)
    {
        Id = id;
        TenantId = tenantId;
        SessionId = sessionId;
        Device = device;
        DeviceInfo = deviceInfo;
        UserId = userId;
        ClientId = clientId;
        IpAddresses = ipAddresses;
        SignedIn = signedIn;
        LastAccessed = lastAccessed;
    }
}
