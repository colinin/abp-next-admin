//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Options;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Volo.Abp.Authorization.Permissions;
//using Volo.Abp.Security.Claims;

//namespace LINGYUN.Abp.ElsaNext.Permissions;

//public class AbpElsaPermissionMapClaimsPrincipalContributor : AbpDynamicClaimsPrincipalContributorBase
//{
//    private const string PermissionClaimType = "permissions";
//    public async override Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
//    {
//        var claimsIdentity = context.ClaimsPrincipal.Identities.First();
//        if (!claimsIdentity.IsAuthenticated)
//        {
//            return;
//        }

//        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpElsaPermissionMapOptions>>();
//        var permissionChecker = context.ServiceProvider.GetRequiredService<IPermissionChecker>();
//        var permissionNames = options.Value.PermissionMaps.Select(x => x.Source).ToArray();
//        var checkResult = await permissionChecker.IsGrantedAsync(context.ClaimsPrincipal, permissionNames);

//        var grantPermissions = checkResult.Result.Where(x => x.Value == PermissionGrantResult.Granted).Select(x => x.Key);

//        // PermissionNames.ClaimType => permissions
//        // Permission.ToString()  => $"{Verb}{Separator}{Resource}";
//        var elsaPermissions = options.Value.PermissionMaps.Where(x => grantPermissions.Contains(x.Source))
//            .Select(x => new AbpDynamicClaim(PermissionClaimType, x.Target))
//            .ToList();

//        await AddDynamicClaimsAsync(context, claimsIdentity, elsaPermissions);
//    }
//}
