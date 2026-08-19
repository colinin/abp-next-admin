using Elastic.Clients.Elasticsearch;
using LINGYUN.Abp.Elasticsearch;
using LINGYUN.Abp.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch
{
    [DependsOn(
        typeof(AbpTestsBaseModule),
        typeof(AbpAuditLoggingElasticsearchModule))]
    public class AbpAuditLoggingElasticsearchTestModule : AbpModule
    {
        private const string UserSecretsId = "1748BEB4-4C7E-46F2-AE59-23956096B8E3";

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
            var options = serviceProvider.GetRequiredService<IOptions<AbpAuditLoggingElasticsearchOptions>>().Value;
            var clientFactory = serviceProvider.GetRequiredService<IElasticsearchClientFactory>();
            var client = clientFactory.Create();
            var indicesResponse = client.Indices.Get($"{options.IndexPrefix}-audit-log");
            if (indicesResponse.IsSuccess())
            {
                foreach (var index in indicesResponse.Indices)
                {
                    client.Indices.Delete(index.Key);
                }
            }
        }
    }
}
