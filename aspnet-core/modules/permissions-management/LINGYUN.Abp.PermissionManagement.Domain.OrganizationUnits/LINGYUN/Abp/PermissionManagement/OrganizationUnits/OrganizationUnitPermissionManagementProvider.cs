using LINGYUN.Abp.Authorization.Permissions;
using LINGYUN.Abp.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Linq;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using UserManager = Volo.Abp.Identity.IdentityUserManager;

namespace LINGYUN.Abp.PermissionManagement.OrganizationUnits;
public class OrganizationUnitPermissionManagementProvider : PermissionManagementProvider
{
    public override string Name => OrganizationUnitPermissionValueProvider.ProviderName;

    protected UserManager UserManager { get; }
    protected IAsyncQueryableExecuter AsyncQueryableExecuter { get; }
    protected IIdentityUserRepository IdentityUserRepository { get; }
    protected IIdentityRoleRepository IdentityRoleRepository { get; }
    protected IRepository<PermissionGrant, Guid> PermissionGrantBasicRepository { get; }

    public OrganizationUnitPermissionManagementProvider(
        IAsyncQueryableExecuter asyncQueryableExecuter,
        IRepository<PermissionGrant, Guid> permissionGrantBasicRepository,
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
        AsyncQueryableExecuter = asyncQueryableExecuter;
        IdentityUserRepository = identityUserRepository;
        IdentityRoleRepository = identityRoleRepository;
        PermissionGrantBasicRepository = permissionGrantBasicRepository;
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
            var roleOrganizationUnits = organizationUnits.Select(x => x.Code.ToString());

            var quaryble = await PermissionGrantBasicRepository.GetQueryableAsync();
            quaryble = quaryble.Where(x => x.ProviderName == Name && roleOrganizationUnits.Contains(x.ProviderKey) && names.Contains(x.Name));
            var roleUnitGrants = await AsyncQueryableExecuter.ToListAsync(quaryble);

            permissionGrants.AddRange(roleUnitGrants);
        }

        if (providerName == UserPermissionValueProvider.ProviderName)
        {
            var userId = Guid.Parse(providerKey);
            var organizationUnits = await IdentityUserRepository.GetOrganizationUnitsAsync(id: userId);
            var userOrganizationUnits = organizationUnits.Select(x => x.Code.ToString());

            var quaryble = await PermissionGrantBasicRepository.GetQueryableAsync();
            quaryble = quaryble.Where(x => x.ProviderName == Name && userOrganizationUnits.Contains(x.ProviderKey) && names.Contains(x.Name));
            var userOrganizationUnitGrants = await AsyncQueryableExecuter.ToListAsync(quaryble);

            permissionGrants.AddRange(userOrganizationUnitGrants);
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
