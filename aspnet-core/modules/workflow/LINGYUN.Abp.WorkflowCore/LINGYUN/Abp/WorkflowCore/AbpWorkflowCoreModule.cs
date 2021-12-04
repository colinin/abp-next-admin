using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Modularity;
using WorkflowCore.Interface;
using WorkflowCore.Services;

namespace LINGYUN.Abp.WorkflowCore
{
    public class AbpWorkflowCoreModule : AbpModule
    {
        private readonly static IList<Type> _definitionWorkflows = new List<Type>();

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AutoAddDefinitionWorkflows(context.Services);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddWorkflow(options =>
            {
                context.Services.ExecutePreConfiguredActions(options);
            });
            context.Services.AddWorkflowDSL();
            //context.Services.AddHostedService((provider) => provider.GetRequiredService<IWorkflowHost>());
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var workflowRegistry = context.ServiceProvider.GetRequiredService<IWorkflowRegistry>();

            foreach (var definitionWorkflow in _definitionWorkflows)
            {
                var workflow = context.ServiceProvider.GetRequiredService(definitionWorkflow);
                workflowRegistry.RegisterWorkflow(workflow as WorkflowBase);
            }

            var workflowHost = context.ServiceProvider.GetRequiredService<IWorkflowHost>();
            workflowHost.Start();
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            var workflowHost = context.ServiceProvider.GetRequiredService<IWorkflowHost>();
            workflowHost.Stop();
        }

        private static void AutoAddDefinitionWorkflows(IServiceCollection services)
        {
            services.OnRegistred(context =>
            {
                if (typeof(WorkflowBase).IsAssignableFrom(context.ImplementationType))
                {
                    _definitionWorkflows.Add(context.ImplementationType);
                }
            });
        }
    }
}
