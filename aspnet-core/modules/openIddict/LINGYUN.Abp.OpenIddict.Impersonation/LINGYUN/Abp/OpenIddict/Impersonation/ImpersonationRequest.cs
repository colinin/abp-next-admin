using System;

namespace LINGYUN.Abp.OpenIddict.Impersonation;

public class ImpersonationRequest
{
    public Guid? UserId { get; set; }
    public Guid? TenantId { get; set; }
    public Guid? UserDelegationId { get; set; }
    public string? TenantUserName { get; set; }

    public bool HasImpersonationTarget()
    {
        return UserId.HasValue || TenantId.HasValue || UserDelegationId.HasValue;
    }

    public bool IsUserImpersonation()
    {
        return UserId.HasValue && !UserDelegationId.HasValue;
    }

    public bool IsTenantImpersonation()
    {
        return !UserId.HasValue && TenantId.HasValue && !UserDelegationId.HasValue;
    }
}