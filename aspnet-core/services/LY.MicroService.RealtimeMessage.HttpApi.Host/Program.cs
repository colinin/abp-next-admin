using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace LY.MicroService.RealtimeMessage;

public class Program
{
    public static int Main(string[] args)
    {
        try
        {
            var hostBuilder = CreateHostBuilder(args).Build();
            Log.Information("Starting MessageService.Host.");
            hostBuilder.Run();

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
       Host.CreateDefaultBuilder(args)
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
