using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.RabbitMQ;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.RabbitMQ
{
    [DependsOn(typeof(AbpRabbitMqModule))]
    [DependsOn(typeof(AbpWorkflowCoreModule))]
    public class AbpWorkflowCoreRabbitMQModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<IQueueProvider, AbpRabbitMqQueueProvider>();
            context.Services.AddSingleton<AbpRabbitMqQueueProvider>();

            PreConfigure<WorkflowOptions>(options =>
            {
                options.UseQueueProvider(provider => provider.GetRequiredService<AbpRabbitMqQueueProvider>());
            });
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            context.ServiceProvider
                .GetRequiredService<AbpRabbitMqQueueProvider>()
                .Dispose();
        }
    }
}
