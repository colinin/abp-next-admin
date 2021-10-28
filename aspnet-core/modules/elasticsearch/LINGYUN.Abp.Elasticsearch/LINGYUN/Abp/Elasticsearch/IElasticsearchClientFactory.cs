using Nest;

namespace LINGYUN.Abp.Elasticsearch
{
    public interface IElasticsearchClientFactory
    {
        IElasticClient Create();
    }
}
