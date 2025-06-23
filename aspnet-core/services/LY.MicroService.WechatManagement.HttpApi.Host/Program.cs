using LY.MicroService.WechatManagement;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using Volo.Abp.IO;
using Volo.Abp.Modularity.PlugIns;

try
{
    Console.Title = "Web.Host";
    Log.Information("Starting web host.");

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
    await builder.AddApplicationAsync<WechatManagementHttpApiHostModule>(options =>
    {
        WechatManagementHttpApiHostModule.ApplicationName = Environment.GetEnvironmentVariable("APPLICATION_NAME")
            ?? WechatManagementHttpApiHostModule.ApplicationName;
        options.ApplicationName = WechatManagementHttpApiHostModule.ApplicationName;
        // �ӻ�������ȡ�û���������, ��������������
        options.Configuration.UserSecretsId = Environment.GetEnvironmentVariable("APPLICATION_USER_SECRETS_ID");
        // �������û��ָ���û�����, ����Ŀ��ȡ
        options.Configuration.UserSecretsAssembly = typeof(WechatManagementHttpApiHostModule).Assembly;
        // ���� Modules Ŀ¼�������ļ���Ϊ���
        // ȡ����ʾ��������������Ŀ��ģ�飬��Ϊͨ���������ʽ����
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
