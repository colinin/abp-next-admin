using JetBrains.Annotations;
using LINGYUN.Abp.Authorization.OrganizationUnits;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Volo.Abp;

namespace System.Security.Principal;

public static class AbpClaimOrganizationUnitsExtensions
{
    public static Guid[] FindOrganizationUnits([NotNull] this ClaimsPrincipal principal)
    {
        Check.NotNull(principal, nameof(principal));

        var userOusOrNull = principal.Claims?.Where(c => c.Type == AbpOrganizationUnitClaimTypes.OrganizationUnit);
        if (userOusOrNull == null || !userOusOrNull.Any())
        {
            return new Guid[0];
        }

        var userOus = new List<Guid>();

        foreach (var userOusClaim in userOusOrNull)
        {
            if (Guid.TryParse(userOusClaim.Value, out var guid))
            {
                userOus.Add(guid);
            }
        }

        return userOus.ToArray();
    }
}
