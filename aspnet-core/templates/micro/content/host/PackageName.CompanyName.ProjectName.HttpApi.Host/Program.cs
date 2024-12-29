using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.IO;
using Volo.Abp.Modularity.PlugIns;

namespace PackageName.CompanyName.ProjectName;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        try
        {
            Console.Title = "Web.Host";
            Log.Information("Starting web host.");

            var builder = WebApplication.CreateBuilder(args);
            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac()
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
            await builder.AddApplicationAsync<ProjectNameHttpApiHostModule>(options =>
            {
                ProjectNameHttpApiHostModule.ApplicationName = Environment.GetEnvironmentVariable("APPLICATION_NAME")
                    ?? ProjectNameHttpApiHostModule.ApplicationName;
                options.ApplicationName = ProjectNameHttpApiHostModule.ApplicationName;
                // 搜索 Modules 目录下所有文件作为插件
                // 取消显示引用所有其他项目的模块，改为通过插件的形式引用
                var pluginFolder = Path.Combine(
                        Directory.GetCurrentDirectory(), "Modules");
                DirectoryHelper.CreateIfNotExists(pluginFolder);
                options.PlugInSources.AddFolder(
                    pluginFolder,
                    SearchOption.AllDirectories);
            });
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
