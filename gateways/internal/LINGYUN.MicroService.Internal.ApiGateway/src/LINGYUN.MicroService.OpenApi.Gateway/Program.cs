using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;

namespace LINGYUN.MicroService.OpenApi.Gateway;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        try
        {
            Log.Information("Starting OpenApi ApiGateway.");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac()
                .AddYarpJson()
                .ConfigureAppConfiguration((context, config) =>
                {
                    var configuration = config.Build();
                    var agileConfigEnabled = configuration["AgileConfig:IsEnabled"];
                    if (agileConfigEnabled.IsNullOrEmpty() || bool.Parse(agileConfigEnabled))
                    {
                        config.AddAgileConfig(new AgileConfig.Client.ConfigClient(configuration));
                    }
                })
                .UseSerilog((context, provider, config) =>
                {
                    config.ReadFrom.Configuration(context.Configuration);
                });

            await builder.AddApplicationAsync<OpenApiGatewayModule>(options =>
            {
                OpenApiGatewayModule.ApplicationName = Environment.GetEnvironmentVariable("APPLICATION_NAME")
                    ?? OpenApiGatewayModule.ApplicationName;
                options.ApplicationName = OpenApiGatewayModule.ApplicationName;
            });
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();

            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Starting OpenApi ApiGateway terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
