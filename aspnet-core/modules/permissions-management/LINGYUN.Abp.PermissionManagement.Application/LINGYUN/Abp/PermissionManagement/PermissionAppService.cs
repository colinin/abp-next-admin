using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SimpleStateChecking;
using VoloPermissionAppService = Volo.Abp.PermissionManagement.PermissionAppService;

namespace LINGYUN.Abp.PermissionManagement;

[Dependency(ReplaceServices = true)]
public class PermissionAppService : VoloPermissionAppService, IPermissionAppService
{
    protected IPermissionGrantRepository PermissionGrantRepository { get; }
    public PermissionAppService(
        IMultiplePermissionManager permissionManager,
        IPermissionChecker permissionChecker,
        IPermissionDefinitionManager permissionDefinitionManager,
        IPermissionGrantRepository permissionGrantRepository,
        IResourcePermissionManager resourcePermissionManager,
        IResourcePermissionGrantRepository resourcePermissionGrantRepository,
        IOptions<PermissionManagementOptions> options,
        ISimpleStateCheckerManager<PermissionDefinition> simpleStateCheckerManager) 
        : base(
            permissionManager,
            permissionChecker,
            permissionDefinitionManager,
            resourcePermissionManager,
            resourcePermissionGrantRepository,
            options,
            simpleStateCheckerManager)
    {
        PermissionGrantRepository = permissionGrantRepository;
    }

    public async virtual Task<GetPermissionGrantedWithProviderListResultDto> GetGrantedByProviderAsync(
        [NotNull] string permissionName, 
        [NotNull] string providerName)
    {
        await CheckProviderPolicy(providerName);

        var permissionGrants = await PermissionGrantRepository.GetListAsync([permissionName], providerName);
        return new GetPermissionGrantedWithProviderListResultDto
        {
            GrantedProviders = permissionGrants.Select(x => new ProviderInfoDto
            {
                ProviderName = x.ProviderName,
                ProviderKey = x.ProviderKey,
            }).ToList(),
        };
    }

    public async override Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
    {
        if (PermissionManager is IMultiplePermissionManager permissionManager)
        {
            await CheckProviderPolicy(providerName);
            await FilterInputPermissionsByCurrentUserAsync(input);

            await permissionManager.SetManyAsync(
                providerName, 
                providerKey, 
                input.Permissions.Select(p => new PermissionChangeState(p.Name, p.IsGranted)));
        }
        else
        {
            await base.UpdateAsync(providerName, providerKey, input);
        }
    }
}
