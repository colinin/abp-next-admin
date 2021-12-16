using LINGYUN.Abp.WorkflowCore.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore
{
    [DependsOn(
        typeof(AbpTimingModule),
        typeof(AbpThreadingModule))]
    public class AbpWorkflowCoreModule : AbpModule
    {
        private readonly static IList<Type> _definitionWorkflows = new List<Type>();

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddConventionalRegistrar(new AbpWorkflowCoreConventionalRegistrar());

            context.Services.AddSingleton<IQueueProvider, AbpUnitOfWorkQueueProvider>();
            context.Services.AddSingleton<AbpUnitOfWorkQueueProvider>();

            PreConfigure<WorkflowOptions>(options =>
            {
                options.UseQueueProvider(provider => provider.GetRequiredService<AbpUnitOfWorkQueueProvider>());
            });

            AutoAddDefinitionWorkflows(context.Services);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddWorkflow(options =>
            {
                context.Services.ExecutePreConfiguredActions(options);
            });
            context.Services.AddWorkflowDSL();

            context.Services.AddWorkflowMiddleware<FeatureCheckWorkflowMiddleware>();

            context.Services.AddWorkflowStepMiddleware<MultiTenancyStepMiddleware>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var workflowCoreOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpWorkflowCoreOptions>>().Value;

            if (workflowCoreOptions.IsEnabled)
            {
                var workflowRegistry = context.ServiceProvider.GetRequiredService<IWorkflowRegistry>();

                foreach (var definitionWorkflow in _definitionWorkflows)
                {
                    var workflow = context.ServiceProvider.GetRequiredService(definitionWorkflow);
                    WorkflowRegisterHelper.RegisterWorkflow(workflowRegistry, workflow);
                }

                var workflowHost = context.ServiceProvider.GetRequiredService<IWorkflowHost>();
                workflowHost.Start();
            }
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            var workflowCoreOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpWorkflowCoreOptions>>().Value;
            if (workflowCoreOptions.IsEnabled)
            {
                var workflowHost = context.ServiceProvider.GetRequiredService<IWorkflowHost>();
                workflowHost.Stop();
            }
        }

        private static void AutoAddDefinitionWorkflows(IServiceCollection services)
        {
            services.OnRegistred(context =>
            {
                if (context.ImplementationType.IsWorkflow())
                {
                    _definitionWorkflows.Add(context.ImplementationType);
                }
            });
        }
    }
}
