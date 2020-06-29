using LINGYUN.Abp.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using IdentityRole = Volo.Abp.Identity.IdentityRole;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace AuthServer.Host.EventBus.Handlers
{
    public class TenantCreateEventHandler : IDistributedEventHandler<CreateEventData>, ITransientDependency
    {
        protected ILogger<TenantCreateEventHandler> Logger { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected IGuidGenerator GuidGenerator { get; }
        protected IdentityUserManager IdentityUserManager { get; }
        protected IdentityRoleManager IdentityRoleManager { get; }

        public TenantCreateEventHandler(
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator,
            IdentityUserManager identityUserManager,
            IdentityRoleManager identityRoleManager,
            ILogger<TenantCreateEventHandler> logger)
        {
            Logger = logger;
            CurrentTenant = currentTenant;
            GuidGenerator = guidGenerator;
            IdentityUserManager = identityUserManager;
            IdentityRoleManager = identityRoleManager;
        }

        [UnitOfWork]
        public async Task HandleEventAsync(CreateEventData eventData)
        {
            using (CurrentTenant.Change(eventData.Id, eventData.Name))
            {
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
                    tenantAdminUser = new IdentityUser(eventData.AdminUserId, eventData.AdminEmailAddress,
                    eventData.AdminEmailAddress, eventData.Id);

                    tenantAdminUser.AddRole(tenantAdminRoleId);

                    // 创建租户管理用户
                    (await IdentityUserManager.CreateAsync(tenantAdminUser)).CheckErrors();
                    (await IdentityUserManager.AddPasswordAsync(tenantAdminUser, eventData.AdminPassword)).CheckErrors();
                }
                //var identitySeedResult = await IdentityDataSeeder
                //   .SeedAsync(eventData.AdminEmailAddress, eventData.AdminPassword, eventData.Id);
                //if (!identitySeedResult.CreatedAdminUser)
                //{
                //    Logger.LogWarning("Tenant {0} admin user {1} not created!", eventData.Name, eventData.AdminEmailAddress);
                //}
                //if (!identitySeedResult.CreatedAdminRole)
                //{
                //    Logger.LogWarning("Tenant {0} admin role not created!", eventData.Name);
                //}
            }
        }
    }
}
