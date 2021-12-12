using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WorkflowManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(WorkflowManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule))]
    public class WorkflowManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<WorkflowManagementDbContext>(options =>
            {
                options.AddRepository<Workflow, EfCoreWorkflowRepository>();
                options.AddRepository<StepNode, EfCoreStepNodeRepository>();
                options.AddRepository<CompensateNode, EfCoreCompensateNodeRepository>();
            });
        }
    }
}
