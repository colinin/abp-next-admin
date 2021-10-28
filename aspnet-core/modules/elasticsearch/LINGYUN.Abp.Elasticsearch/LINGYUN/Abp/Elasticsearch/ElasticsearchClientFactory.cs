using Microsoft.Extensions.Options;
using Nest;
using System;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Elasticsearch
{
    public class ElasticsearchClientFactory : IElasticsearchClientFactory, ISingletonDependency
    {
        private readonly AbpElasticsearchOptions _options;
        private readonly Lazy<IElasticClient> _lazyClient;

        public ElasticsearchClientFactory(
            IOptions<AbpElasticsearchOptions> options)
        {
            _options = options.Value;

            _lazyClient = new Lazy<IElasticClient>(CreateClient);
        }

        public IElasticClient Create() => _lazyClient.Value;

        protected virtual IElasticClient CreateClient()
        {
            var configuration = _options.CreateConfiguration();

            var client = new ElasticClient(configuration);

            return client;
        }
    }
}
