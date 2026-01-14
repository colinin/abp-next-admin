using Elastic.Clients.Elasticsearch;

namespace LINGYUN.Abp.Elasticsearch
{
    public interface IElasticsearchClientFactory
    {
        ElasticsearchClient Create();
    }
}
