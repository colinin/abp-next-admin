using JetBrains.Annotations;
using LINGYUN.Abp.Authorization.OrganizationUnits;
using System;
using System.Collections.Generic;

namespace Volo.Abp.Users;

public static class CurrentUserOrganizationUnitsExtensions
{
    public static Guid[] FindOrganizationUnits([NotNull] this ICurrentUser currentUser)
    {
        var organizationUnits = currentUser.FindClaims(AbpOrganizationUnitClaimTypes.OrganizationUnit);
        if (organizationUnits.IsNullOrEmpty())
        {
            return new Guid[0];
        }

        var userOus = new List<Guid>();

        foreach (var userOusClaim in organizationUnits)
        {
            if (Guid.TryParse(userOusClaim.Value, out var guid))
            {
                userOus.Add(guid);
            }
        }

        return userOus.ToArray();
    }
}
