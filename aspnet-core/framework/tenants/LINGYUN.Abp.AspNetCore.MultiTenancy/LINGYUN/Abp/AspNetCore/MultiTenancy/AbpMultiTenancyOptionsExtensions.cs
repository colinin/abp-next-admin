using System.Collections.Generic;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.AspNetCore.MultiTenancy;

public static class AbpMultiTenancyOptionsExtensions
{
    public static void AddOnlyDomainTenantResolver(this AbpTenantResolveOptions options, string domainFormat)
    {
        options.TenantResolvers.InsertAfter(
            r => r is CurrentUserTenantResolveContributor,
            new OnlyDomainTenantResolveContributor(domainFormat)
        );
    }
}
