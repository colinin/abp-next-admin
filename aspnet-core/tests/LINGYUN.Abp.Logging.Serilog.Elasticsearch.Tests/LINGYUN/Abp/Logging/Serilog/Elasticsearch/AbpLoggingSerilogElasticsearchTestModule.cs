using Elastic.Clients.Elasticsearch;
using LINGYUN.Abp.Elasticsearch;
using LINGYUN.Abp.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Logging.Serilog.Elasticsearch;

[DependsOn(
    typeof(AbpTestsBaseModule),
    typeof(AbpLoggingTestModule),
    typeof(AbpLoggingSerilogElasticsearchModule))]
public class AbpLoggingSerilogElasticsearchTestModule : AbpModule
{
    private const string UserSecretsId = "11A604D4-3A64-4F92-94C6-5B1525CF63DD";

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
        {
            builder.AddUserSecrets(UserSecretsId);
        }));
    }

    public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
    {
        RemoveTestIndexs(context.ServiceProvider);
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        RemoveTestIndexs(context.ServiceProvider);
    }

    private static void RemoveTestIndexs(IServiceProvider serviceProvider)
    {
        var clientFactory = serviceProvider.GetRequiredService<IElasticsearchClientFactory>();
        var client = clientFactory.Create();
        var indicesResponse = client.Indices.Get("abp-test-logging");
        if (indicesResponse.IsSuccess())
        {
            foreach (var index in indicesResponse.Indices)
            {
                client.Indices.Delete(index.Key);
            }
        }
    }
}
