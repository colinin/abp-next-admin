using System;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Serializable]
    public class LoadBalancerOptionsDto
    {
        public string Type { get; set; }
        public string Key { get; set; }
        public int? Expiry { get; set; }
    }
}