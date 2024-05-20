using LINGYUN.Abp.Authorization.OrganizationUnits;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace LINGYUN.Abp.Identity.OrganizationUnits;

public class OrganizationUnitClaimsPrincipalContributor : IAbpClaimsPrincipalContributor, ITransientDependency
{
    // https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Extensions.Core/src/UserClaimsPrincipalFactory.cs#L74
    // private static string IdentityAuthenticationType => "Identity.Application";

    private readonly IIdentityUserRepository _identityUserRepository;
    private readonly IIdentityRoleRepository _identityRoleRepository;

    public OrganizationUnitClaimsPrincipalContributor(
        IIdentityUserRepository identityUserRepository,
        IIdentityRoleRepository identityRoleRepository)
    {
        _identityUserRepository = identityUserRepository;
        _identityRoleRepository = identityRoleRepository;
    }

    public async virtual Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        var claimsIdentity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        if (claimsIdentity == null)
        {
            return;
        }
        if (claimsIdentity.FindAll(x => x.Type == AbpOrganizationUnitClaimTypes.OrganizationUnit).Any())
        {
            return;
        }
        var userId = claimsIdentity.FindUserId();
        if (!userId.HasValue)
        {
            return;
        }

        var userOus = await _identityUserRepository.GetOrganizationUnitsAsync(id: userId.Value);

        foreach (var userOu in userOus)
        {
            if (!claimsIdentity.HasClaim(AbpOrganizationUnitClaimTypes.OrganizationUnit, userOu.Code))
            {
                claimsIdentity.AddClaim(new Claim(AbpOrganizationUnitClaimTypes.OrganizationUnit, userOu.Code));
            }
        }

        var userRoles = claimsIdentity
            .FindAll(x => x.Type == AbpClaimTypes.Role)
            .Select(x => x.Value)
            .Distinct();

        var roleOus = await _identityRoleRepository.GetOrganizationUnitsAsync(userRoles);
        foreach (var roleOu in roleOus)
        {
            if (!claimsIdentity.HasClaim(AbpOrganizationUnitClaimTypes.OrganizationUnit, roleOu.Code))
            {
                claimsIdentity.AddClaim(new Claim(AbpOrganizationUnitClaimTypes.OrganizationUnit, roleOu.Code));
            }
        }

        context.ClaimsPrincipal.AddIdentityIfNotContains(claimsIdentity);
    }
}
