using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.MultiTenancy;
using LY.MicroService.IdentityServer.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using IdentityRole = Volo.Abp.Identity.IdentityRole;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LY.MicroService.IdentityServer.EventBus.Handlers;

public class TenantSynchronizer : IDistributedEventHandler<CreateEventData>, ITransientDependency
{
    protected ILogger<TenantSynchronizer> Logger { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IdentityUserManager IdentityUserManager { get; }
    protected IdentityRoleManager IdentityRoleManager { get; }
    protected IPermissionDataSeeder PermissionDataSeeder { get; }
    protected IDbSchemaMigrator DbSchemaMigrator { get; }

    public TenantSynchronizer(
        ICurrentTenant currentTenant,
        IGuidGenerator guidGenerator,
        IdentityUserManager identityUserManager,
        IdentityRoleManager identityRoleManager,
        IPermissionDataSeeder permissionDataSeeder,
        IDbSchemaMigrator dbSchemaMigrator,
        ILogger<TenantSynchronizer> logger)
    {
        Logger = logger;
        CurrentTenant = currentTenant;
        GuidGenerator = guidGenerator;
        IdentityUserManager = identityUserManager;
        IdentityRoleManager = identityRoleManager;
        PermissionDataSeeder = permissionDataSeeder;
        DbSchemaMigrator = dbSchemaMigrator;
    }

    [UnitOfWork]
    public async Task HandleEventAsync(CreateEventData eventData)
    {
        using (CurrentTenant.Change(eventData.Id, eventData.Name))
        {
            Logger.LogInformation("Migrating the new tenant database with AuthServer...");
            // 迁移租户数据
            await DbSchemaMigrator.MigrateAsync<IdentityServertMigrationsDbContext>(
                (connectionString, builder) =>
                {
                    builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

                    return new IdentityServertMigrationsDbContext(builder.Options);
                });

            Logger.LogInformation("Migrated the new tenant database with AuthServer.");

            Logger.LogInformation("Seeding the new tenant admin user and roles...");
            await SeedTenantAdminAsync(eventData);

            Logger.LogInformation("Seeding the new tenant default roles...");
            await SeedTenantDefaultRoleAsync(eventData.Id);

            Logger.LogInformation("The new tenant identity data initialized!");
        }
    }

    private async Task SeedTenantDefaultRoleAsync(Guid tenantId)
    {
        // 默认用户
        var roleId = GuidGenerator.Create();
        var defaultRole = new IdentityRole(roleId, "Users", tenantId)
        {
            IsStatic = true,
            IsPublic = true,
            IsDefault = true,
        };
        (await IdentityRoleManager.CreateAsync(defaultRole)).CheckErrors();

        // 所有用户都应该具有查询用户权限, 用于IM场景
        await PermissionDataSeeder.SeedAsync(
            RolePermissionValueProvider.ProviderName,
            defaultRole.Name,
            new string[] {
                    IdentityPermissions.UserLookup.Default,
                    IdentityPermissions.Users.Default
            });
    }

    private async Task SeedTenantAdminAsync(CreateEventData eventData)
    {
        const string tenantAdminUserName = "admin";
        const string tenantAdminRoleName = "admin";
        var tenantAdminRoleId = Guid.Empty; ;

        if (!await IdentityRoleManager.RoleExistsAsync(tenantAdminRoleName))
        {
            tenantAdminRoleId = GuidGenerator.Create();
            var tenantAdminRole = new IdentityRole(tenantAdminRoleId, tenantAdminRoleName, eventData.Id)
            {
                IsStatic = true,
                IsPublic = true
            };
            (await IdentityRoleManager.CreateAsync(tenantAdminRole)).CheckErrors();
        }
        else
        {
            var tenantAdminRole = await IdentityRoleManager.FindByNameAsync(tenantAdminRoleName);
            tenantAdminRoleId = tenantAdminRole.Id;
        }

        var tenantAdminUser = await IdentityUserManager.FindByNameAsync(eventData.AdminEmailAddress);
        if (tenantAdminUser == null)
        {
            tenantAdminUser = new IdentityUser(
                eventData.AdminUserId,
                tenantAdminUserName,
                eventData.AdminEmailAddress,
                eventData.Id);

            tenantAdminUser.AddRole(tenantAdminRoleId);

            // 创建租户管理用户
            (await IdentityUserManager.CreateAsync(tenantAdminUser)).CheckErrors();
            (await IdentityUserManager.AddPasswordAsync(tenantAdminUser, eventData.AdminPassword)).CheckErrors();
        }
    }
}
