using System;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Serializable]
    public class HostAndPortDto
    {
        public string Host { get; set; }
        public int? Port { get; set; }
    }
}