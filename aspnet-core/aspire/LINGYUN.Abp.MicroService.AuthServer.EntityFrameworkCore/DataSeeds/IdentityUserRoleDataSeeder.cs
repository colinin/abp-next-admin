using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.MicroService.AuthServer.DataSeeds;
public class IdentityUserRoleDataSeeder : ITransientDependency
{
    public const string AdminUserIdPropertyName = "AdminUserId";
    public const string AdminEmailPropertyName = "AdminEmail";
    public const string AdminEmailDefaultValue = "admin@abp.io";
    public const string AdminUserNamePropertyName = "AdminUserName";
    public const string AdminUserNameDefaultValue = "admin";
    public const string AdminPasswordPropertyName = "AdminPassword";
    public const string AdminPasswordDefaultValue = "1q2w3E*";

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
    }

    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await SeedAdminUserAsync(context);
        await SeedDefaultRoleAsync(context);
    }

    private async Task SeedAdminUserAsync(DataSeedContext context)
    {
        await IdentityOptions.SetAsync();

        const string adminRoleName = "admin";
        var adminUserName = context?[AdminUserNamePropertyName] as string ?? AdminUserNameDefaultValue;

        Guid adminRoleId;
        if (!await RoleManager.RoleExistsAsync(adminRoleName))
        {
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

        var adminUser = await UserManager.FindByNameAsync(adminUserName);
        if (adminUser == null)
        {
            adminUser = new IdentityUser(
                adminUserId,
                adminUserName,
                adminEmailAddress,
                context.TenantId);

            adminUser.AddRole(adminRoleId);

            // 创建租户管理用户
            (await UserManager.CreateAsync(adminUser)).CheckErrors();
            (await UserManager.AddPasswordAsync(adminUser, adminPassword)).CheckErrors();
        }
    }

    private async Task SeedDefaultRoleAsync(DataSeedContext context)
    {
        const string defaultRoleName = "Users";
        if (await RoleManager.FindByNameAsync(defaultRoleName) != null)
        {
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
