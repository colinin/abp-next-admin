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
            context.Services.AddSingleton<IPersistenceProvider, AbpWorkflowPersistenceProvider>();
            context.Services.AddSingleton<AbpWorkflowPersistenceProvider>();

            PreConfigure<WorkflowOptions>(options =>
            {
                options.UsePersistence(provider => provider.GetRequiredService<AbpWorkflowPersistenceProvider>());
            });
        }
    }
}
