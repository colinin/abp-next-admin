using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elasticsearch.EsqlQuery;

[DependsOn(typeof(AbpElasticsearchModule))]
public class AbpElasticsearchEsqlQueryModule : AbpModule
{
}
