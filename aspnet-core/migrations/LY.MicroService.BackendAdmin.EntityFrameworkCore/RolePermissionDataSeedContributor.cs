using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;

namespace LY.MicroService.BackendAdmin.EntityFrameworkCore;

public class RolePermissionDataSeedContributor : IDataSeedContributor
{
    public ILogger<RolePermissionDataSeedContributor> Logger { protected get; set; }

    protected ICurrentTenant CurrentTenant { get; }
    protected IPermissionDataSeeder PermissionDataSeeder { get; }
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

    public RolePermissionDataSeedContributor(
        ICurrentTenant currentTenant, 
        IPermissionDataSeeder permissionDataSeeder, 
        IPermissionDefinitionManager permissionDefinitionManager)
    {
        CurrentTenant = currentTenant;
        PermissionDataSeeder = permissionDataSeeder;
        PermissionDefinitionManager = permissionDefinitionManager;

        Logger = NullLogger<RolePermissionDataSeedContributor>.Instance;
    }

    public async virtual Task SeedAsync(DataSeedContext context)
    {
        using (CurrentTenant.Change(context.TenantId))
        {
            Logger.LogInformation("Seeding the new tenant admin role permissions...");

            var definitionPermissions = await PermissionDefinitionManager.GetPermissionsAsync();
            await PermissionDataSeeder.SeedAsync(
                RolePermissionValueProvider.ProviderName,
                "admin",
                definitionPermissions.Select(x => x.Name),
                context.TenantId);
        }
    }
}
