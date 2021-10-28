using LINGYUN.Abp.Elasticsearch;
using LINGYUN.Abp.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch
{
    [DependsOn(
        typeof(AbpTestsBaseModule),
        typeof(AbpAuditLoggingElasticsearchModule))]
    public class AbpAuditLoggingElasticsearchTestModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configurationOptions = new AbpConfigurationBuilderOptions
            {
                BasePath = @"D:\Projects\Development\Abp\AuditLogging\Elasticsearch",
                EnvironmentName = "Development"
            };

            context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(configurationOptions));
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<AbpAuditLoggingElasticsearchOptions>>().Value;
            var clientFactory = context.ServiceProvider.GetRequiredService<IElasticsearchClientFactory>();
            var client = clientFactory.Create();
            var indicesResponse = client.Cat.Indices(i => i.Index($"{options.IndexPrefix}-security-log"));
            foreach (var index in indicesResponse.Records)
            {
                client.Indices.Delete(index.Index);
            }
        }
    }
}
