using Grpc.Net.Client;

namespace LINGYUN.Abp.Dapr.Client
{
    public class AbpDaprClientOptions
    {
        public string GrpcEndpoint { get; set; }
        public string HttpEndpoint { get; set; }
        public GrpcChannelOptions GrpcChannelOptions { get; set; }
        public AbpDaprClientOptions()
        {
        }
    }
}
