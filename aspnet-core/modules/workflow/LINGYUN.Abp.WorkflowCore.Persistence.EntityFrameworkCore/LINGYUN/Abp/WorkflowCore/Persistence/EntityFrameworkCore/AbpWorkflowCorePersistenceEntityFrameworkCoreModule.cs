using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WorkflowCore.Persistence.EntityFrameworkCore
{
    [DependsOn(typeof(AbpWorkflowCorePersistenceModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class AbpWorkflowCorePersistenceEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<WorkflowDbContext>(options =>
            {
                options.AddRepository<PersistedWorkflow, EfCoreWorkflowRepository>();
                options.AddRepository<PersistedEvent, EfCoreWorkflowEventRepository>();
                options.AddRepository<PersistedExecutionError, EfCoreWorkflowExecutionErrorRepository>();
                options.AddRepository<PersistedScheduledCommand, EfCoreWorkflowScheduledCommandRepository>();
                options.AddRepository<PersistedSubscription, EfCoreWorkflowEventSubscriptionRepository>();
            });
        }
    }
}
