using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace LINGYUN.Abp.MicroService.ServiceDefaults;
public static class HealthChecksExtenssions
{
    public static IHostApplicationBuilder AddCustomHealthChecks<THealthCheck>(
        this IHostApplicationBuilder builder, 
        string name,
        string tag = "ready") 
        where THealthCheck : class, IHealthCheck
    {
        builder.Services
            .AddHealthChecks()
            .AddCheck<THealthCheck>(name, tags: [tag]);

        return builder;
    }

    public static WebApplication MapCustomHealthChecks(
        this WebApplication app,
        string pattern,
        string tag = "ready")
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapHealthChecks(pattern, new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains(tag)
            });
        }
        return app;
    }
}
