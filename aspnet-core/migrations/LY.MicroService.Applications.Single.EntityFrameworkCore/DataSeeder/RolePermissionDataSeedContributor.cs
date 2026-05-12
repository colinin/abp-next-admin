using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.DataSeeder;

public class RolePermissionDataSeedContributor : IDataSeedContributor, ITransientDependency
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
            Logger.LogInformation("Seeding the Users role permissions...");

            // 所有用户都应该具有查询用户权限, 用于IM场景
            await PermissionDataSeeder.SeedAsync(
                RolePermissionValueProvider.ProviderName,
                "Users",
                new string[]
                {
                    IdentityPermissions.UserLookup.Default,
                    IdentityPermissions.Users.Default,
                    "Platform.Feedback.Create"
                },
                tenantId: context.TenantId);

            Logger.LogInformation("Seeding Users role permissions completed.");
        }
    }
}
