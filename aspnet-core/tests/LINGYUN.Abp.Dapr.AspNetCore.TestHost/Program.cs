using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace LINGYUN.Abp.Dapr.AspNetCore.TestHost
{
    public class Program
    {
        public static int Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            return 0;
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
           Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseAutofac();
    }
}
