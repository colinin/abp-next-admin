using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.DataProtection;

[Serializable]
[EventName("abp.data_protection.entity_auth_created")]
public class DataAccessEntityAuthCreateEvent : IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string[] EntityKeys { get; set; }
    public string EntityKeyType { get; set; }
    public string EntityType { get; set; }
    public string[] Roles { get; set; }
    public string[] OrganizationUnits { get; set; }
    public DataAccessEntityAuthCreateEvent()
    {

    }
    public DataAccessEntityAuthCreateEvent(
        string entityType,
        string entityKeyType,
        string[] entityKeys,
        string[] roles = null, 
        string[] organizationUnits = null,
        Guid? tenantId = null)
    {
        EntityType = entityType;
        EntityKeyType = entityKeyType;
        EntityKeys = entityKeys;
        EntityType = entityType;
        Roles = roles;
        OrganizationUnits = organizationUnits;
        TenantId = tenantId;
    }
}
