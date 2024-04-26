using LINGYUN.Abp.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore.Migrations;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using IdentityPermissions = Volo.Abp.Identity.IdentityPermissions;

namespace LY.MicroService.AuthServer.EntityFrameworkCore;

public class AuthServerDbMigrationEventHandler : EfCoreDatabaseMigrationEventHandlerBase<AuthServerMigrationsDbContext>
{
    protected IGuidGenerator GuidGenerator { get; }
    protected IdentityUserManager IdentityUserManager { get; }
    protected IdentityRoleManager IdentityRoleManager { get; }
    protected IPermissionDataSeeder PermissionDataSeeder { get; }

    public AuthServerDbMigrationEventHandler(
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        ITenantStore tenantStore,
        IDistributedEventBus distributedEventBus,
        ILoggerFactory loggerFactory,
        IGuidGenerator guidGenerator,
        IdentityUserManager identityUserManager,
        IdentityRoleManager identityRoleManager,
        IPermissionDataSeeder permissionDataSeeder) 
        : base(
            ConnectionStringNameAttribute.GetConnStringName<AuthServerMigrationsDbContext>(), 
            currentTenant, unitOfWorkManager, tenantStore, distributedEventBus, loggerFactory)
    {
        GuidGenerator = guidGenerator;
        IdentityUserManager = identityUserManager;
        IdentityRoleManager = identityRoleManager;
        PermissionDataSeeder = permissionDataSeeder;
    }

    protected async override Task AfterTenantCreated(TenantCreatedEto eventData, bool schemaMigrated)
    {
        if (!schemaMigrated)
        {
            return;
        }

        using (CurrentTenant.Change(eventData.Id))
        {
            await SeedTenantDefaultRoleAsync(eventData);
            await SeedTenantAdminAsync(eventData);
        }
    }

    protected async virtual Task SeedTenantDefaultRoleAsync(TenantCreatedEto eventData)
    {
        // 默认用户
        var roleId = GuidGenerator.Create();
        var defaultRole = new IdentityRole(roleId, "Users", eventData.Id)
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
            new string[]
            {
                IdentityPermissions.UserLookup.Default,
                IdentityPermissions.Users.Default
            },
            tenantId: eventData.Id);
    }

    protected async virtual Task SeedTenantAdminAsync(TenantCreatedEto eventData)
    {
        const string tenantAdminUserName = "admin";
        const string tenantAdminRoleName = "admin";
        Guid tenantAdminRoleId;
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

        var adminUserId = GuidGenerator.Create();
        if (eventData.Properties.TryGetValue("AdminUserId", out var userIdString) &&
            Guid.TryParse(userIdString, out var adminUserGuid))
        {
            adminUserId = adminUserGuid;
        }
        var adminEmailAddress = eventData.Properties.GetOrDefault("AdminEmail") ?? "admin@abp.io";
        var adminPassword = eventData.Properties.GetOrDefault("AdminPassword") ?? "1q2w3E*";

        var tenantAdminUser = await IdentityUserManager.FindByNameAsync(adminEmailAddress);
        if (tenantAdminUser == null)
        {
            tenantAdminUser = new IdentityUser(
                adminUserId,
                tenantAdminUserName,
                adminEmailAddress,
                eventData.Id);

            tenantAdminUser.AddRole(tenantAdminRoleId);

            // 创建租户管理用户
            (await IdentityUserManager.CreateAsync(tenantAdminUser)).CheckErrors();
            (await IdentityUserManager.AddPasswordAsync(tenantAdminUser, adminPassword)).CheckErrors();
        }
    }
}
