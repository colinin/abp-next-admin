using System;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Serializable]
    public class CacheOptionsDto
    {
        public int? TtlSeconds { get; set; }
        public string Region { get; set; }
    }
}
