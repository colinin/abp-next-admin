using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Identity.Session;

[EventName("abp.identity.session.change_accessed")]
public class IdentitySessionChangeAccessedEvent : IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string SessionId { get; set; }
    public string IpAddresses { get; set; }
    public DateTime LastAccessed { get; set; }
    public IdentitySessionChangeAccessedEvent()
    {

    }
    public IdentitySessionChangeAccessedEvent(
        string sessionId, 
        string ipAddresses, 
        DateTime lastAccessed,
        Guid? tenantId = null)
    {
        SessionId = sessionId;
        IpAddresses = ipAddresses;
        LastAccessed = lastAccessed;
        TenantId = tenantId;
    }
}
