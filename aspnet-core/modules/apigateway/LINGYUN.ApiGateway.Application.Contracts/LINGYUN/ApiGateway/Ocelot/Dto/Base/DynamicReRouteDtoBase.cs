using System;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Serializable]
    public class DynamicReRouteDtoBase
    {
        public string ServiceName { get; set; }
        public string DownstreamHttpVersion { get; set; }
        public RateLimitRuleDto RateLimitRule { get; set; }

        public DynamicReRouteDtoBase()
        {
            RateLimitRule = new RateLimitRuleDto();
        }
    }
}
