using JetBrains.Annotations;
using LINGYUN.Abp.Authorization.OrganizationUnits;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Users;

public static class CurrentUserOrganizationUnitsExtensions
{
    public static string[] FindOrganizationUnits([NotNull] this ICurrentUser currentUser)
    {
        var organizationUnits = currentUser.FindClaims(AbpOrganizationUnitClaimTypes.OrganizationUnit);
        if (organizationUnits.IsNullOrEmpty())
        {
            return new string[0];
        }

        return organizationUnits.Select(x => x.Value).ToArray();
    }
}
