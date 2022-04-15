using LINGYUN.Abp.Authorization.Permissions;
using LINGYUN.Abp.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using UserManager = Volo.Abp.Identity.IdentityUserManager;

namespace LINGYUN.Abp.PermissionManagement.OrganizationUnits;
public class OrganizationUnitPermissionManagementProvider : PermissionManagementProvider
{
    public override string Name => OrganizationUnitPermissionValueProvider.ProviderName;

    protected UserManager UserManager { get; }
    protected IIdentityUserRepository IdentityUserRepository { get; }
    protected IIdentityRoleRepository IdentityRoleRepository { get; }

    public OrganizationUnitPermissionManagementProvider(
        IPermissionGrantRepository permissionGrantRepository,
        IIdentityUserRepository identityUserRepository,
        IIdentityRoleRepository identityRoleRepository,
        UserManager userManager,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant)
        : base(
            permissionGrantRepository,
            guidGenerator,
            currentTenant)
    {
        UserManager = userManager;
        IdentityUserRepository = identityUserRepository;
        IdentityRoleRepository = identityRoleRepository;
    }

    public override async Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, string providerKey)
    {
        var multipleGrantInfo = await CheckAsync(new[] { name }, providerName, providerKey);

        return multipleGrantInfo.Result.Values.First();
    }

    public override async Task<MultiplePermissionValueProviderGrantInfo> CheckAsync(string[] names, string providerName, string providerKey)
    {
        var multiplePermissionValueProviderGrantInfo = new MultiplePermissionValueProviderGrantInfo(names);
        var permissionGrants = new List<PermissionGrant>();

        if (providerName == Name)
        {
            permissionGrants.AddRange(await PermissionGrantRepository.GetListAsync(names, providerName, providerKey));

        }

        if (providerName == RolePermissionValueProvider.ProviderName)
        {
            var role = await IdentityRoleRepository.FindByNormalizedNameAsync(UserManager.NormalizeName(providerKey));
            var organizationUnits = await IdentityRoleRepository.GetOrganizationUnitsAsync(role.Id);

            foreach (var organizationUnit in organizationUnits)
            {
                permissionGrants.AddRange(await PermissionGrantRepository.GetListAsync(names, Name, organizationUnit.Id.ToString()));
            }
        }

        if (providerName == UserPermissionValueProvider.ProviderName)
        {
            var userId = Guid.Parse(providerKey);
            var organizationUnits = await IdentityUserRepository.GetOrganizationUnitsAsync(userId);

            foreach (var organizationUnit in organizationUnits)
            {
                permissionGrants.AddRange(await PermissionGrantRepository.GetListAsync(names, Name, organizationUnit.Id.ToString()));
            }
        }

        permissionGrants = permissionGrants.Distinct().ToList();
        if (!permissionGrants.Any())
        {
            return multiplePermissionValueProviderGrantInfo;
        }

        foreach (var permissionName in names)
        {
            var permissionGrant = permissionGrants.FirstOrDefault(x => x.Name == permissionName);
            if (permissionGrant != null)
            {
                multiplePermissionValueProviderGrantInfo.Result[permissionName] = new PermissionValueProviderGrantInfo(true, permissionGrant.ProviderKey);
            }
        }

        return multiplePermissionValueProviderGrantInfo;
    }
}
