using Volo.Abp.Security.Claims;

namespace System.Security.Claims;

public static class ImpersonationClaimExtensions
{
    public static bool IsImpersonating(this ClaimsPrincipal principal)
    {
        return principal.FindImpersonatorUserId().HasValue;
    }

    public static Guid? FindImpersonatorUserId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue(AbpClaimTypes.ImpersonatorUserId);
        return Guid.TryParse(value, out var result) ? result : null;
    }

    public static Guid? FindImpersonatorTenantId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue(AbpClaimTypes.ImpersonatorTenantId);
        return Guid.TryParse(value, out var result) ? result : null;
    }
}
