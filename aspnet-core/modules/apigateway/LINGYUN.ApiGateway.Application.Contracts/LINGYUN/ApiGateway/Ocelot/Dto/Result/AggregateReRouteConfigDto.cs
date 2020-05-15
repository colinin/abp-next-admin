using System;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Serializable]
    public class AggregateReRouteConfigDto
    {
        public string ReRouteKey { get; set; }
        public string Parameter { get; set; }
        public string JsonPath { get; set; }
    }
}
