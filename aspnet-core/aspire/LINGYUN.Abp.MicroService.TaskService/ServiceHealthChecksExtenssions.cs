using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LINGYUN.Abp.MicroService.TaskService;

internal static class ServiceHealthChecksExtenssions
{
    private const string HealthEndpointPath = "/service-health";
    private const string DefaultHealthTag = "ready";

    public static TBuilder AddServiceHealthChecks<TBuilder>(this TBuilder builder, string name = "Service", string tag = DefaultHealthTag) where TBuilder : IHostApplicationBuilder
    {
        builder.Services
            .AddHealthChecks()
            .AddCheck<ServiceHealthCheck>(name, tags: [tag]);

        return builder;
    }

    public static WebApplication MapServiceHealthChecks(this WebApplication app, string tag = DefaultHealthTag)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapHealthChecks(HealthEndpointPath, new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains(tag)
            });
        }
        return app;
    }
}
