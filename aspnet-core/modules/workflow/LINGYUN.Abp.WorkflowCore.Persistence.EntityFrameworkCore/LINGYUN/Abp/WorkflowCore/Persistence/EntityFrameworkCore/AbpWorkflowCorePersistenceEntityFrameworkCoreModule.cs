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
                options.AddRepository<Workflow, EfCoreWorkflowRepository>();
                options.AddRepository<WorkflowEvent, EfCoreWorkflowEventRepository>();
                options.AddRepository<WorkflowExecutionError, EfCoreWorkflowExecutionErrorRepository>();
                options.AddRepository<WorkflowScheduledCommand, EfCoreWorkflowScheduledCommandRepository>();
                options.AddRepository<WorkflowEventSubscription, EfCoreWorkflowEventSubscriptionRepository>();
            });
        }
    }
}
