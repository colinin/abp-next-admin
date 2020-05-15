using System;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Serializable]
    public class RateLimitOptionsDto
    {
        public string ClientIdHeader { get; set; }
        public string QuotaExceededMessage { get; set; }
        public string RateLimitCounterPrefix { get; set; }
        public bool DisableRateLimitHeaders { get; set; }
        public int? HttpStatusCode { get; set; }
    }
}