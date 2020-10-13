using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace AuthServer.Host
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                Log.Information("Starting web host.");
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
                .UseSerilog((ctx, cfg) =>
                {
                    cfg = cfg.ReadFrom.Configuration(ctx.Configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseAutofac();
    }
}
