using LINGYUN.Common.EventBus.Tenants;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.MultiTenancy;

namespace AuthServer.Host.EventBus.Handlers
{
    public class TenantCreateEventHandler : IDistributedEventHandler<CreateEventData>, ITransientDependency
    {
        protected ILogger<TenantCreateEventHandler> Logger { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected IIdentityDataSeeder IdentityDataSeeder { get; }

        public TenantCreateEventHandler(
            ICurrentTenant currentTenant,
            IIdentityDataSeeder identityDataSeeder,
            ILogger<TenantCreateEventHandler> logger)
        {
            Logger = logger;
            CurrentTenant = currentTenant;
            IdentityDataSeeder = identityDataSeeder;
        }

        public async Task HandleEventAsync(CreateEventData eventData)
        {
            using (CurrentTenant.Change(eventData.Id, eventData.Name))
            {
                var identitySeedResult = await IdentityDataSeeder
                   .SeedAsync(eventData.AdminEmailAddress, eventData.AdminPassword, eventData.Id);
                if (!identitySeedResult.CreatedAdminUser)
                {
                    Logger.LogWarning("Tenant {0} admin user {1} not created!", eventData.Name, eventData.AdminEmailAddress);
                }
                if (!identitySeedResult.CreatedAdminRole)
                {
                    Logger.LogWarning("Tenant {0} admin role not created!", eventData.Name);
                }
            }
        }
    }
}
