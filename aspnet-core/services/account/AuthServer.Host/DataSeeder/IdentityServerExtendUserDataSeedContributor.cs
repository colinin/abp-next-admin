
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using IdentityRole = Volo.Abp.Identity.IdentityRole;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace AuthServer.DataSeeder
{
    public class IdentityServerExtendUserDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        public const string AdminEmailPropertyName = "AdminEmail";
        public const string AdminEmailDefaultValue = "vben@abp.io";
        public const string AdminPasswordPropertyName = "AdminPassword";
        public const string AdminPasswordDefaultValue = "1q2w3E*";
        public const string AdminRolePropertyName = "AdminRole";
        public const string AdminRoleDefaultValue = "vben-admin";

        protected IGuidGenerator GuidGenerator { get; }
        protected IIdentityRoleRepository RoleRepository { get; }
        protected IIdentityUserRepository UserRepository { get; }
        protected ILookupNormalizer LookupNormalizer { get; }
        protected IdentityUserManager UserManager { get; }
        protected IdentityRoleManager RoleManager { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected IOptions<IdentityOptions> IdentityOptions { get; }

        public IdentityServerExtendUserDataSeedContributor(
            IGuidGenerator guidGenerator,
            IIdentityRoleRepository roleRepository,
            IIdentityUserRepository userRepository,
            ILookupNormalizer lookupNormalizer,
            IdentityUserManager userManager,
            IdentityRoleManager roleManager,
            ICurrentTenant currentTenant,
            IOptions<IdentityOptions> identityOptions)
        {
            GuidGenerator = guidGenerator;
            RoleRepository = roleRepository;
            UserRepository = userRepository;
            LookupNormalizer = lookupNormalizer;
            UserManager = userManager;
            RoleManager = roleManager;
            CurrentTenant = currentTenant;
            IdentityOptions = identityOptions;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            using (CurrentTenant.Change(context.TenantId))
            {
                await IdentityOptions.SetAsync();

                var result = new IdentityDataSeedResult();
                //"admin" user
                const string adminUserName = "vben";
                var adminEmail = context?[AdminEmailPropertyName] as string ?? AdminEmailDefaultValue;
                var adminPassword = context?[AdminPasswordPropertyName] as string ?? AdminPasswordDefaultValue;

                var adminUser = await UserRepository.FindByNormalizedUserNameAsync(
                    LookupNormalizer.NormalizeName(adminUserName)
                );

                if (adminUser != null)
                {
                    return;
                }

                adminUser = new IdentityUser(
                    GuidGenerator.Create(),
                    adminUserName,
                    adminEmail,
                    context.TenantId
                )
                {
                    Name = adminUserName
                };

                (await UserManager.CreateAsync(adminUser, adminPassword, validatePassword: false)).CheckErrors();
                result.CreatedAdminUser = true;

                //"admin" role
                var adminRoleName = context?[AdminRolePropertyName] as string ?? AdminRoleDefaultValue;
                var adminRole =
                    await RoleRepository.FindByNormalizedNameAsync(LookupNormalizer.NormalizeName(adminRoleName));
                if (adminRole == null)
                {
                    adminRole = new IdentityRole(
                        GuidGenerator.Create(),
                        adminRoleName,
                        context.TenantId
                    )
                    {
                        IsStatic = true,
                        IsPublic = true
                    };

                    (await RoleManager.CreateAsync(adminRole)).CheckErrors();
                    result.CreatedAdminRole = true;
                }

                (await UserManager.AddToRoleAsync(adminUser, adminRoleName)).CheckErrors();
            }
        }
    }
}
