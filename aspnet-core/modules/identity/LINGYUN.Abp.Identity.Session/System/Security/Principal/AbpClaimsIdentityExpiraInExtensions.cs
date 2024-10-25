using JetBrains.Annotations;
using System.Linq;
using System.Security.Claims;
using Volo.Abp;

namespace System.Security.Principal;
public static class AbpClaimsIdentityExpiraInExtensions
{
    public static long? FindExpirainTime([NotNull] this ClaimsPrincipal principal)
    {
        return principal.FindLongClaimValue("exp");
    }

    public static long? FindIssuedTime([NotNull] this ClaimsPrincipal principal)
    {
        return principal.FindLongClaimValue("iat");
    }

    public static long? FindLongClaimValue([NotNull] this ClaimsPrincipal principal, string claimType)
    {
        Check.NotNull(principal, nameof(principal));

        var longValueOrNull = principal.Claims?.FirstOrDefault(c => c.Type == claimType);
        if (longValueOrNull == null || longValueOrNull.Value.IsNullOrWhiteSpace())
        {
            return null;
        }

        if (long.TryParse(longValueOrNull.Value, out var longValue))
        {
            return longValue;
        }

        return null;
    }
}
