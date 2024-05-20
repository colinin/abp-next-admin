using JetBrains.Annotations;
using LINGYUN.Abp.Authorization.OrganizationUnits;
using System.Linq;
using System.Security.Claims;
using Volo.Abp;

namespace System.Security.Principal;

public static class AbpClaimOrganizationUnitsExtensions
{
    public static string[] FindOrganizationUnits([NotNull] this ClaimsPrincipal principal)
    {
        Check.NotNull(principal, nameof(principal));

        var userOusOrNull = principal.Claims?.Where(c => c.Type == AbpOrganizationUnitClaimTypes.OrganizationUnit);
        if (userOusOrNull == null || !userOusOrNull.Any())
        {
            return new string[0];
        }

        return userOusOrNull.Select(x => x.Value).ToArray();
    }
}
