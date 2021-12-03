using LINGYUN.Abp.Elasticsearch;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Elasticsearch
{
    [DependsOn(typeof(AbpWorkflowCoreModule))]
    [DependsOn(typeof(AbpElasticsearchModule))]
    public class AbpWorkflowCoreElasticsearchModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<ISearchIndex, AbpElasticsearchIndexer>();
            context.Services.AddSingleton<AbpElasticsearchIndexer>();

            PreConfigure<WorkflowOptions>(options =>
            {
                options.UseSearchIndex(provider => provider.GetRequiredService<AbpElasticsearchIndexer>());
            });
        }
    }
}
