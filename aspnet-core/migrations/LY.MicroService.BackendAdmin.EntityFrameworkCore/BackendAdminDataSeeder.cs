using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;

namespace LY.MicroService.BackendAdmin.EntityFrameworkCore;

public class BackendAdminDataSeeder : ITransientDependency
{
    protected ILogger<BackendAdminDataSeeder> Logger { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
    protected IPermissionDataSeeder PermissionDataSeeder { get; }

    public BackendAdminDataSeeder(
        IPermissionDefinitionManager permissionDefinitionManager,
        IPermissionDataSeeder permissionDataSeeder,
        ICurrentTenant currentTenant)
    {
        PermissionDefinitionManager = permissionDefinitionManager;
        PermissionDataSeeder = permissionDataSeeder;
        CurrentTenant = currentTenant;

        Logger = NullLogger<BackendAdminDataSeeder>.Instance;
    }

    public virtual async Task SeedAsync(DataSeedContext context)
    {
        using (CurrentTenant.Change(context.TenantId))
        {
            await SeedAdminRolePermissionsAsync(context);
        }
    }

    private async Task SeedAdminRolePermissionsAsync(DataSeedContext context)
    {
        Logger.LogInformation("Seeding the default role permissions...");

        var multiTenancySide = CurrentTenant.GetMultiTenancySide();
        var permissionNames = (await PermissionDefinitionManager.GetPermissionsAsync())
            .Where(p => p.MultiTenancySide.HasFlag(multiTenancySide))
            .Where(p => !p.Providers.Any() || p.Providers.Contains(RolePermissionValueProvider.ProviderName))
            .Select(p => p.Name)
            .ToArray();

        await PermissionDataSeeder.SeedAsync(
            RolePermissionValueProvider.ProviderName,
            "admin",
            permissionNames,
            context?.TenantId
        );

        Logger.LogInformation("Seed default role permissions completed.");
    }
}
