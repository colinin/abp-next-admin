using System;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Serializable]
    public class HttpHandlerOptionsDto
    {
        public bool AllowAutoRedirect { get; set; }

        public bool UseCookieContainer { get; set; }

        public bool UseTracing { get; set; }

        public bool UseProxy { get; set; }

        public int? MaxConnectionsPerServer { get; set; }
    }
}