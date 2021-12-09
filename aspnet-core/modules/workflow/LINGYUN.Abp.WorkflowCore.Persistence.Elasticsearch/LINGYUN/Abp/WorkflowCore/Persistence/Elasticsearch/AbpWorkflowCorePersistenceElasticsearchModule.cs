using LINGYUN.Abp.Elasticsearch;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Persistence.Elasticsearch
{
    [DependsOn(typeof(AbpWorkflowCoreModule))]
    [DependsOn(typeof(AbpJsonModule))]
    [DependsOn(typeof(AbpElasticsearchModule))]
    public class AbpWorkflowCorePersistenceElasticsearchModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IPersistenceProvider, ElasticsearchPersistenceProvider>();
            context.Services.AddTransient<ElasticsearchPersistenceProvider>();

            PreConfigure<WorkflowOptions>(options =>
            {
                options.UsePersistence(provider => provider.GetRequiredService<ElasticsearchPersistenceProvider>());
            });
        }
    }
}
