using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using static Elastic.Clients.Elasticsearch.ElasticsearchClientSettings;

namespace LINGYUN.Abp.Elasticsearch
{
    public class AbpElasticsearchOptions
    {
        /// <summary>
        /// 字段名称 是否为 camelCase 格式
        /// 如果为true, 在ES中为 camelCase
        /// 如果为false，在ES中为 CamelCase
        /// 默认：false
        /// </summary>
        public bool FieldCamelCase { get; set; }
        /// <summary>
        /// When set to true will disable (de)serializing directly to the request and response stream and return a byte[] copy of the raw request and response. 
        /// Defaults to false.
        /// </summary>
        public bool DisableDirectStreaming { get; set; }
        public string NodeUris { get; set; }
        public int ConnectionLimit { get; set; }
        /// <summary>
        /// `Base64ApiKey` for Elastic Cloud style encoded api keys
        /// </summary>
        public string? Base64ApiKey { get; set; }
        /// <summary>
        /// `ApiKey` for simple secret token
        /// </summary>
        public string? ApiKey { get; set; }
        /// <summary>
        /// `UserName` for basic authentication
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// `Password` for basic authentication
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// Default: 60s
        /// </summary>
        public TimeSpan RequestTimeout { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IRequestInvoker RequestInvoker { get; set; }
        public SourceSerializerFactory SerializerFactory { get; set; }

        public AbpElasticsearchOptions()
        {
            ConnectionLimit = TransportConfiguration.DefaultConnectionLimit;
            // Default: 60s
            // See: https://www.elastic.co/docs/reference/elasticsearch/clients/dotnet/_options_on_elasticsearchclientsettings
            RequestTimeout = TimeSpan.FromSeconds(60);
        }

        internal IElasticsearchClientSettings CreateClientSettings()
        {
            NodePool nodePool;
            IEnumerable<Uri> nodes = NodeUris
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(uriString => new Uri(uriString));
            if (nodes.Count() == 1)
            {
                nodePool = new SingleNodePool(nodes.First());
            }
            else
            {
                nodePool = new StaticNodePool(nodes);
            }

            var clientSettings = new ElasticsearchClientSettings(
                nodePool,
                RequestInvoker,
                SerializerFactory)
                .ConnectionLimit(ConnectionLimit)
                .RequestTimeout(RequestTimeout);

            if (!FieldCamelCase)
            {
                clientSettings.DefaultFieldNameInferrer((name) => name);
            }

            if (!Base64ApiKey.IsNullOrWhiteSpace())
            {
                clientSettings.Authentication(new Base64ApiKey(Base64ApiKey));
            }
            else if (!ApiKey.IsNullOrWhiteSpace())
            {
                clientSettings.Authentication(new ApiKey(ApiKey));
            }
            else if (!UserName.IsNullOrWhiteSpace() && !Password.IsNullOrWhiteSpace())
            {
                clientSettings.Authentication(new BasicAuthentication(UserName, Password));
            }

            clientSettings.DisableDirectStreaming(DisableDirectStreaming);

            return clientSettings;
        }
    }
}
