using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace LY.MicroService.AuthServer.EntityFrameworkCore.DataSeeds;

public class IdentityUserRoleDataSeeder : ITransientDependency
{
    public const string AdminUserIdPropertyName = "AdminUserId";
    public const string AdminEmailPropertyName = "AdminEmail";
    public const string AdminEmailDefaultValue = "admin@abp.io";
    public const string AdminUserNamePropertyName = "AdminUserName";
    public const string AdminUserNameDefaultValue = "admin";
    public const string AdminPasswordPropertyName = "AdminPassword";
    public const string AdminPasswordDefaultValue = "1q2w3E*";

    public ILogger<IdentityUserRoleDataSeeder> Logger { protected get; set; }

    protected IGuidGenerator GuidGenerator { get; }
    protected IIdentityRoleRepository RoleRepository { get; }
    protected IIdentityUserRepository UserRepository { get; }
    protected ILookupNormalizer LookupNormalizer { get; }
    protected IdentityUserManager UserManager { get; }
    protected IdentityRoleManager RoleManager { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }

    public IdentityUserRoleDataSeeder(
        IGuidGenerator guidGenerator,
        IIdentityRoleRepository roleRepository,
        IIdentityUserRepository userRepository,
        ILookupNormalizer lookupNormalizer,
        IdentityUserManager userManager,
        IdentityRoleManager roleManager,
        IOptions<IdentityOptions> identityOptions)
    {
        GuidGenerator = guidGenerator;
        RoleRepository = roleRepository;
        UserRepository = userRepository;
        LookupNormalizer = lookupNormalizer;
        UserManager = userManager;
        RoleManager = roleManager;
        IdentityOptions = identityOptions;

        Logger = NullLogger<IdentityUserRoleDataSeeder>.Instance;
    }

    public virtual async Task SeedAsync(DataSeedContext context)
    {
        Logger.LogInformation("Seeding the admin user roles...");
        await SeedAdminUserAsync(context);

        Logger.LogInformation("Seeding the default roles...");
        await SeedDefaultRoleAsync(context);

        Logger.LogInformation("Seeding user roles completed.");
    }

    private async Task SeedAdminUserAsync(DataSeedContext context)
    {
        await IdentityOptions.SetAsync();

        const string adminRoleName = "admin";
        var adminUserName = context?[AdminUserNamePropertyName] as string ?? AdminUserNameDefaultValue;

        Guid adminRoleId;

        Logger.LogInformation("Check admin role {adminRoleName} exists.", adminRoleName);

        if (!await RoleManager.RoleExistsAsync(adminRoleName))
        {
            Logger.LogInformation("Create new role {adminRoleName}.", adminRoleName);

            adminRoleId = GuidGenerator.Create();
            var adminRole = new IdentityRole(
                adminRoleId,
                adminRoleName,
                context.TenantId)
            {
                IsStatic = true,
                IsPublic = true
            };
            (await RoleManager.CreateAsync(adminRole)).CheckErrors();
        }
        else
        {
            var adminRole = await RoleManager.FindByNameAsync(adminRoleName);
            adminRoleId = adminRole.Id;
        }

        var adminUserId = GuidGenerator.Create();
        if (context.Properties.TryGetValue(AdminUserIdPropertyName, out var userIdString) &&
            Guid.TryParse(userIdString?.ToString(), out var adminUserGuid))
        {
            adminUserId = adminUserGuid;
        }
        var adminEmailAddress = context?[AdminEmailPropertyName] as string ?? AdminEmailDefaultValue;
        var adminPassword = context?[AdminPasswordPropertyName] as string ?? AdminPasswordDefaultValue;

        Logger.LogInformation("Check admin user {adminUserName} exists.", adminUserName);

        var adminUser = await UserManager.FindByNameAsync(adminUserName);
        if (adminUser == null)
        {
            Logger.LogInformation("Create new user {adminUserName}.", adminUserName);

            adminUser = new IdentityUser(
                adminUserId,
                adminUserName,
                adminEmailAddress,
                context.TenantId);

            adminUser.AddRole(adminRoleId);

            // 创建租户管理用户
            (await UserManager.CreateAsync(adminUser)).CheckErrors();

            Logger.LogInformation("Add user {adminUserName} password.", adminUserName);
            (await UserManager.AddPasswordAsync(adminUser, adminPassword)).CheckErrors();
        }
    }

    private async Task SeedDefaultRoleAsync(DataSeedContext context)
    {
        const string defaultRoleName = "Users";
        Logger.LogInformation("Check Role {defaultRoleName} exists.", defaultRoleName);
        if (!await RoleManager.RoleExistsAsync(defaultRoleName))
        {
            Logger.LogInformation("Create new role {defaultRoleName}.", defaultRoleName);
            var roleId = GuidGenerator.Create();
            var defaultRole = new IdentityRole(
                roleId,
                defaultRoleName,
                context.TenantId)
            {
                IsStatic = true,
                IsPublic = true,
                IsDefault = true,
            };
            (await RoleManager.CreateAsync(defaultRole)).CheckErrors();
        }
    }
}
