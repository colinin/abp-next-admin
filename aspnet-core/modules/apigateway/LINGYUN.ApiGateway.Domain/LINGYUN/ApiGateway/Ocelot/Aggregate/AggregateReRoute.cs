using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateway.Ocelot
{
    // TODO: 聚合暂时不实现
    public class AggregateReRoute : AggregateRoot<int>
    {
        public virtual string AppId { get; protected set; }
        /// <summary>
        /// 聚合名称
        /// </summary>
        public virtual string Name { get; protected set; }
        public virtual long ReRouteId { get; private set; }
        public virtual string ReRouteKeys { get; private set; }
        public virtual string UpstreamPathTemplate { get; private set; }
        public virtual string UpstreamHost { get; private set; }
        public virtual bool ReRouteIsCaseSensitive { get; private set; }
        public virtual string Aggregator { get; private set; }
        public virtual int? Priority { get; set; }
        public virtual string UpstreamHttpMethod { get; private set; }
        public virtual ICollection<AggregateReRouteConfig> ReRouteKeysConfig { get; private set; }
        protected AggregateReRoute()
        {
            ReRouteKeysConfig = new List<AggregateReRouteConfig>();
        }

        public AggregateReRoute SetUpstreamPath(string host, string path, string appId)
        {
            AppId = appId;
            UpstreamHost = host;
            UpstreamPathTemplate = path;
            return this;
        }

        public AggregateReRoute(string name, long routeId, string aggregator) : this()
        {
            Name = name;
            ReRouteId = routeId;
            Aggregator = aggregator;
        }

        public AggregateReRoute AddUpstreamHttpMethod(string method)
        {
            if (!UpstreamHttpMethod.Contains(method))
            {
                UpstreamHttpMethod += "," + method;
            }
            return this;
        }

        public AggregateReRoute RemoveUpstreamHttpMethod(string method)
        {
            if (!UpstreamHttpMethod.Contains(method))
            {
                var removeMethod = "," + method;
                UpstreamHttpMethod = UpstreamHttpMethod.Replace(removeMethod, "");
            }
            return this;
        }

        public AggregateReRoute AddRouteKey(string key)
        {
            if (!ReRouteKeys.Contains(key))
            {
                ReRouteKeys += "," + key;
            }
            return this;
        }

        public AggregateReRoute RemoveRouteKey(string key)
        {
            if (!ReRouteKeys.Contains(key))
            {
                var removeKey = "," + key;
                ReRouteKeys = ReRouteKeys.Replace(removeKey, "");
            }
            return this;
        }

        public AggregateReRoute AddReRouteConfig(string routeKey, string paramter, string jsonPath)
        {
            if (!ReRouteKeysConfig.Any(k => k.ReRouteKey.Equals(routeKey)))
            {
                var aggregateReRouteConfig = new AggregateReRouteConfig(ReRouteId);
                aggregateReRouteConfig.ApplyReRouteConfig(routeKey, paramter, jsonPath);
                ReRouteKeysConfig.Add(aggregateReRouteConfig);
            }
            return this;
        }

        public AggregateReRoute RemoveReRouteConfig(string routeKey)
        {
            ReRouteKeysConfig.RemoveAll(k => k.ReRouteKey.Equals(routeKey));
            return this;
        }
    }
}
