using Elsa.Extensions;
using Elsa.Features.Services;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.Server;

[DependsOn(typeof(AbpElsaNextModule))]
public class AbpElsaNextServerModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration.GetSection("Elsa");

        PreConfigure<IModule>(elsa =>
        {
            // see: https://v3.elsaworkflows.io/docs/installation/elsa-server

            // Use timer activities.
            elsa.UseScheduling();

            // Enable JavaScript workflow expressions.
            elsa.UseJavaScript();

            // Enable Liquid workflow expressions.
            elsa.UseLiquid();

            // Enable C# workflow expressions
            elsa.UseCSharp();

            // Enable HTTP activities.
            elsa.UseHttp(http =>
            {
                http.ConfigureHttpOptions = options => configuration.GetSection("Http").Bind(options);
            });

            // Expose Elsa API endpoints.
            elsa.UseWorkflowsApi();

            // Operational dashboard API 
            elsa.UseDashboardApi();

            if (configuration["Server:EnableRealTimeWorkflows"] != "false")
            {
                elsa.UseRealTimeWorkflows();
            }

            // Register custom activities from the application, if any.
            elsa.AddActivitiesFrom<AbpElsaNextServerModule>();

            // Register custom workflows from the application, if any.
            elsa.AddWorkflowsFrom<AbpElsaNextServerModule>();
        });
    }
}
