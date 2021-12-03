using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.LifeCycleEvent
{
    [DependsOn(typeof(AbpEventBusModule))]
    [DependsOn(typeof(AbpWorkflowCoreModule))]
    public class AbpWorkflowCoreLifeCycleEventModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<ILifeCycleEventHub, AbpEventBusLifeCycleEventHub>();
            context.Services.AddSingleton<AbpEventBusLifeCycleEventHub>();

            PreConfigure<WorkflowOptions>(options =>
            {
                options.UseEventHub(provider => provider.GetRequiredService<AbpEventBusLifeCycleEventHub>());
            });
        }
    }
}
