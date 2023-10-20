using LINGYUN.Abp.Authorization.OrganizationUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;

namespace LINGYUN.Abp.Authorization.Permissions;

public class OrganizationUnitPermissionValueProvider : PermissionValueProvider
{
    public const string ProviderName = "O";

    public override string Name => ProviderName;

    public OrganizationUnitPermissionValueProvider(
        IPermissionStore permissionStore)
        : base(permissionStore)
    {
    }

    public async override Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
    {
        var organizationUnits = context.Principal?.FindAll(AbpOrganizationUnitClaimTypes.OrganizationUnit).Select(c => c.Value).ToArray();

        if (organizationUnits == null || !organizationUnits.Any())
        {
            return PermissionGrantResult.Undefined;
        }

        foreach (var organizationUnit in organizationUnits.Distinct())
        {
            if (await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, organizationUnit))
            {
                return PermissionGrantResult.Granted;
            }
        }

        return PermissionGrantResult.Undefined;
    }

    public async override Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context)
    {
        var permissionNames = context.Permissions.Select(x => x.Name).Distinct().ToList();
        Check.NotNullOrEmpty(permissionNames, nameof(permissionNames));

        var result = new MultiplePermissionGrantResult(permissionNames.ToArray());

        var organizationUnits = context.Principal?.FindAll(AbpOrganizationUnitClaimTypes.OrganizationUnit).Select(c => c.Value).ToArray();
        if (organizationUnits == null || !organizationUnits.Any())
        {
            return result;
        }

        foreach (var organizationUnit in organizationUnits.Distinct())
        {
            var multipleResult = await PermissionStore.IsGrantedAsync(permissionNames.ToArray(), Name, organizationUnit);
              
            foreach (var grantResult in multipleResult.Result.Where(grantResult =>
                result.Result.ContainsKey(grantResult.Key) &&
                result.Result[grantResult.Key] == PermissionGrantResult.Undefined &&
                grantResult.Value != PermissionGrantResult.Undefined))
            {
                result.Result[grantResult.Key] = grantResult.Value;
                permissionNames.RemoveAll(x => x == grantResult.Key);
            }

            if (result.AllGranted || result.AllProhibited)
            {
                break;
            }

            if (permissionNames.IsNullOrEmpty())
            {
                break;
            }
        }

        return result;
    }
}
