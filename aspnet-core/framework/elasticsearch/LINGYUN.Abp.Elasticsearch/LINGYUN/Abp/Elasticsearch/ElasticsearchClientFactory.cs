using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Options;
using System;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Elasticsearch
{
    public class ElasticsearchClientFactory : IElasticsearchClientFactory, ISingletonDependency
    {
        private readonly AbpElasticsearchOptions _options;
        private readonly Lazy<ElasticsearchClient> _lazyClient;

        public ElasticsearchClientFactory(
            IOptions<AbpElasticsearchOptions> options)
        {
            _options = options.Value;

            _lazyClient = new Lazy<ElasticsearchClient>(CreateClient);
        }

        public ElasticsearchClient Create() => _lazyClient.Value;

        protected virtual ElasticsearchClient CreateClient()
        {
            var configuration = _options.CreateClientSettings();

            var client = new ElasticsearchClient(configuration);

            return client;
        }
    }
}
