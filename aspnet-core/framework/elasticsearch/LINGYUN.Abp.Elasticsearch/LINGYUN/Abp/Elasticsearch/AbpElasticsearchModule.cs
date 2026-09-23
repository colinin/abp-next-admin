using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elasticsearch;

[DependsOn(typeof(AbpCachingModule))]
public class AbpElasticsearchModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<AbpElasticsearchOptions>(configuration.GetSection("Elasticsearch"));
    }
}
