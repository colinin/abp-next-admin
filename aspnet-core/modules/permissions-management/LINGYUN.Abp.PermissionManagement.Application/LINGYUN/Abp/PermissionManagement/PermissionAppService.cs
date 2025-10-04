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
[ExposeServices(
    typeof(IPermissionAppService), 
    typeof(VoloPermissionAppService),
    typeof(PermissionAppService))]
public class PermissionAppService : VoloPermissionAppService
{
    public PermissionAppService(
        IMultiplePermissionManager permissionManager, 
        IPermissionDefinitionManager permissionDefinitionManager,
        IOptions<PermissionManagementOptions> options,
        ISimpleStateCheckerManager<PermissionDefinition> simpleStateCheckerManager) 
        : base(permissionManager, permissionDefinitionManager, options, simpleStateCheckerManager)
    {
    }

    public async override Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
    {
        if (PermissionManager is IMultiplePermissionManager permissionManager)
        {
            await CheckProviderPolicy(providerName);

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
