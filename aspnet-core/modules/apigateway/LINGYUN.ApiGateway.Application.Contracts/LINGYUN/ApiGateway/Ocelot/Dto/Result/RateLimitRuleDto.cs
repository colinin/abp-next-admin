using System;
using System.Collections.Generic;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Serializable]
    public class RateLimitRuleDto
    {
        public List<string> ClientWhitelist { get; set; }

        public bool EnableRateLimiting { get; set; }

        public string Period { get; set; }

        public double? PeriodTimespan { get; set; }

        public long? Limit { get; set; }
        public RateLimitRuleDto()
        {
            ClientWhitelist = new List<string>();
        }
    }
}