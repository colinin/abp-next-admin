using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace LY.MicroService.WorkflowManagement;

public class Program
{
    public static int Main(string[] args)
    {
        try
        {
            var host = CreateHostBuilder(args).Build();
            Log.Information("Starting web host.");
            host.Run();
            return 0;
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
            .UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            })
            .UseAutofac();
}
