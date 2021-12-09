using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace LY.MicroService.BackendAdmin;

public class Program
{
    public static int Main(string[] args)
    {
        try
        {
            Log.Information("Starting BackendAdmin.Host.");
            CreateHostBuilder(args).Build().Run();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    internal static IHostBuilder CreateHostBuilder(string[] args) =>
       Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureAppConfiguration((context, config) =>
            {
                var configuration = config.Build();
                if (configuration.GetSection("AgileConfig").Exists())
                {
                    config.AddAgileConfig(new AgileConfig.Client.ConfigClient(configuration));
                }
            })
            .UseSerilog((context, provider, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            })
            .UseAutofac();
}
