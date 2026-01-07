using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System;

namespace Microsoft.Extensions.Hosting;

// Adds common Aspire services: service discovery, resilience, health checks, and OpenTelemetry.
// This project should be referenced by each service project in your solution.
// To learn more about using this project, see https://aka.ms/dotnet/aspire/service-defaults
public static class Extensions
{
    private const string HealthEndpointPath = "/health";
    private const string AlivenessEndpointPath = "/alive";

    public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.ConfigureOpenTelemetry();

        builder.ReplaceDefaultConfiguration();

        builder.AddDefaultHealthChecks();

        builder.Services.AddServiceDiscovery();

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on resilience by default
            http.AddStandardResilienceHandler();

            // Turn on service discovery by default
            http.AddServiceDiscovery();
        });

        // Uncomment the following to restrict the allowed schemes for service discovery.
        // builder.Services.Configure<ServiceDiscoveryOptions>(options =>
        // {
        //     options.AllowedSchemes = ["https"];
        // });

        return builder;
    }

    public static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.Services
            .AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();
            })
            .WithTracing(tracing =>
            {
                tracing.AddSource(builder.Environment.ApplicationName)
                    .AddAspNetCoreInstrumentation(tracing =>
                        // Exclude health check requests from tracing
                        tracing.Filter = context =>
                            !context.Request.Path.StartsWithSegments(HealthEndpointPath)
                            && !context.Request.Path.StartsWithSegments(AlivenessEndpointPath)
                    )
                    // Uncomment the following line to enable gRPC instrumentation (requires the OpenTelemetry.Instrumentation.GrpcNetClient package)
                    //.AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddCapInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation();
            });

        builder.AddOpenTelemetryExporters();

        return builder;
    }

    public static TBuilder ReplaceDefaultConfiguration<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.AddRedisClient("redis");
        builder.AddRabbitMQClient("rabbitmq");
        builder.AddElasticsearchClient("elasticsearch");

        // CAP PostgreSql
        builder.Configuration["CAP:PostgreSql:ConnectionString"] = builder.Configuration.GetConnectionString("Default");

        // CAP RabbitMQ
        builder.Configuration["CAP:RabbitMQ:HostName"] = builder.Configuration["RABBITMQ_HOST"];
        builder.Configuration["CAP:RabbitMQ:UserName"] = builder.Configuration["RABBITMQ_USERNAME"];
        builder.Configuration["CAP:RabbitMQ:Password"] = builder.Configuration["RABBITMQ_PASSWORD"];
        builder.Configuration["CAP:RabbitMQ:Port"] = builder.Configuration["RABBITMQ_PORT"];

        // Abp RabbitMQ
        builder.Configuration["RabbitMQ:Default:HostName"] = builder.Configuration["RABBITMQ_HOST"];
        builder.Configuration["RabbitMQ:Default:UserName"] = builder.Configuration["RABBITMQ_USERNAME"];
        builder.Configuration["RabbitMQ:Default:Password"] = builder.Configuration["RABBITMQ_PASSWORD"];
        builder.Configuration["RabbitMQ:Default:Port"] = builder.Configuration["RABBITMQ_PORT"];

        // Elsa RabbitMQ
        builder.Configuration["Elsa:Rebus:RabbitMQ:Connection"] = builder.Configuration.GetConnectionString("RabbitMQ");

        // Redis
        var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
        builder.Configuration["Redis:Configuration"] = $"{redisConnectionString},defaultDatabase=10";

        // DistributedLock
        builder.Configuration["DistributedLock:Redis:Configuration"] = $"{redisConnectionString},defaultDatabase=11";

        // Features
        builder.Configuration["Features:Validation:Redis:Configuration"] = $"{redisConnectionString},defaultDatabase=12";

        // SignalR
        builder.Configuration["SignalR:Redis:Configuration"] = $"{redisConnectionString},defaultDatabase=13,channelPrefix=abp-realtime-channel";

        // Elasticsearch
        var elasticSearchUrl = builder.Configuration.GetConnectionString("Elasticsearch");
        var serialogEsConfig = builder.Configuration.GetSection("Serilog:WriteTo");

        void ReplaceElasticsearchLogging(IConfiguration configuration)
        {
            foreach (var config in configuration.GetChildren())
            {
                if (string.Equals(config["Name"], "Async", StringComparison.InvariantCultureIgnoreCase))
                {
                    var configureArgs = config.GetSection("Args:configure");
                    ReplaceElasticsearchLogging(configureArgs);
                }
                if (string.Equals(config["Name"], "Elasticsearch", StringComparison.InvariantCultureIgnoreCase))
                {
                    config["Args:nodeUris"] = elasticSearchUrl;
                }
            }
        }

        if (serialogEsConfig.Exists())
        {
            ReplaceElasticsearchLogging(serialogEsConfig);
        }
        builder.Configuration["Elasticsearch:NodeUris"] = elasticSearchUrl;

        return builder;
    }

    private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        // Uncomment the following lines to enable the Azure Monitor exporter (requires the Azure.Monitor.OpenTelemetry.AspNetCore package)
        //if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
        //{
        //    builder.Services.AddOpenTelemetry()
        //       .UseAzureMonitor();
        //}

        return builder;
    }

    public static TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddHealthChecks()
            // Add a default liveness check to ensure app is responsive
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        // Adding health checks endpoints to applications in non-development environments has security implications.
        // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
        if (app.Environment.IsDevelopment())
        {
            // All health checks must pass for app to be considered ready to accept traffic after starting
            app.MapHealthChecks(HealthEndpointPath);

            // Only health checks tagged with the "live" tag must pass for app to be considered alive
            app.MapHealthChecks(AlivenessEndpointPath, new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live")
            });
        }

        return app;
    }
}
