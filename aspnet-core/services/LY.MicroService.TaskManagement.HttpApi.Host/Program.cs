﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.IO;
using Volo.Abp.Modularity.PlugIns;

namespace LY.MicroService.TaskManagement;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        try
        {
            Console.Title = "TaskManagement.HttpApi.Host";
            Log.Information("Starting TaskManagement.HttpApi.Host.");

            var builder = WebApplication.CreateBuilder(args);
            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac()
                .ConfigureAppConfiguration((context, config) =>
                {
                    if (context.Configuration.GetValue("AgileConfig:IsEnabled", false))
                    {
                        config.AddAgileConfig(new AgileConfig.Client.ConfigClient(context.Configuration));
                    }
                })
                .UseSerilog((context, provider, config) =>
                {
                    config.ReadFrom.Configuration(context.Configuration);
                });
            await builder.AddApplicationAsync<TaskManagementHttpApiHostModule>(options =>
            {
                TaskManagementHttpApiHostModule.ApplicationName = Environment.GetEnvironmentVariable("APPLICATION_NAME")
                    ?? TaskManagementHttpApiHostModule.ApplicationName;
                options.ApplicationName = TaskManagementHttpApiHostModule.ApplicationName;
                // 从环境变量取用户机密配置, 适用于容器测试
                options.Configuration.UserSecretsId = Environment.GetEnvironmentVariable("APPLICATION_USER_SECRETS_ID");
                // 如果容器没有指定用户机密, 从项目读取
                options.Configuration.UserSecretsAssembly = typeof(TaskManagementHttpApiHostModule).Assembly;
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
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
            Console.WriteLine("Host terminated unexpectedly!");
            Console.WriteLine(ex.ToString());
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
