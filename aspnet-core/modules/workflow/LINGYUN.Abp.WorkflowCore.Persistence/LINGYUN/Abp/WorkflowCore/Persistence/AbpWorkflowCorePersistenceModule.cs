using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    [DependsOn(typeof(AbpWorkflowCoreModule))]
    public class AbpWorkflowCorePersistenceModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IWorkflowPurger, AbpWorkflowPurger>();
            context.Services.AddTransient<IPersistenceProvider, AbpWorkflowPersistenceProvider>();
            context.Services.AddTransient<AbpWorkflowPersistenceProvider>();

            PreConfigure<WorkflowOptions>(options =>
            {
                options.UsePersistence(provider => provider.GetRequiredService<AbpWorkflowPersistenceProvider>());
            });
        }
    }
}
