using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Modularity;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.DistributedLock
{
    [DependsOn(typeof(AbpWorkflowCoreModule))]
    [DependsOn(typeof(AbpDistributedLockingModule))]
    public class AbpWorkflowCoreDistributedLockModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMemoryCache();
            context.Services.AddSingleton<IDistributedLockProvider, AbpDistributedLockProvider>();
            context.Services.AddSingleton<AbpDistributedLockProvider>();

            PreConfigure<WorkflowOptions>(options =>
            {
                options.UseDistributedLockManager(provider => provider.GetRequiredService<AbpDistributedLockProvider>());
            });
        }
    }
}
