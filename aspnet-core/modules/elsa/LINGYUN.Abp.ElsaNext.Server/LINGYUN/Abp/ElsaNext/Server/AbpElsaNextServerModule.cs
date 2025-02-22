using Elsa.Extensions;
using Elsa.Features.Services;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.Server;

[DependsOn(typeof(AbpElsaNextModule))]
public class AbpElsaNextServerModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IModule>(elsa =>
        {
            // see: https://v3.elsaworkflows.io/docs/installation/elsa-server

            // Expose Elsa API endpoints.
            elsa.UseWorkflowsApi();

            // Setup a SignalR hub for real-time updates from the server.
            elsa.UseRealTimeWorkflows();

            // Enable C# workflow expressions
            elsa.UseCSharp();

            // Enable Liquid workflow expressions.
            elsa.UseLiquid();

            // Enable HTTP activities.
            elsa.UseHttp();

            // Use timer activities.
            elsa.UseScheduling();

            // Register custom activities from the application, if any.
            elsa.AddActivitiesFrom<AbpElsaNextServerModule>();

            // Register custom workflows from the application, if any.
            elsa.AddWorkflowsFrom<AbpElsaNextServerModule>();
        });
    }
}
