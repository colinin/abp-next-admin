using System;
using System.Collections.Generic;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Serializable]
    public class AggregateReRouteDtoBase
    {
        public List<string> ReRouteKeys { get; set; }
        public List<AggregateReRouteConfigDto> ReRouteKeysConfig { get; set; }
        public string UpstreamPathTemplate { get; set; }
        public string UpstreamHost { get; set; }
        public bool ReRouteIsCaseSensitive { get; set; }
        public string Aggregator { get; set; }
        public int Priority { get; set; }
        public List<string> UpstreamHttpMethod { get; set; }
        public AggregateReRouteDtoBase()
        {
            ReRouteKeys = new List<string>();
            UpstreamHttpMethod = new List<string>();
            ReRouteKeysConfig = new List<AggregateReRouteConfigDto>();
        }
    }
}
