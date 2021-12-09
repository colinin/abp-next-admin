using LINGYUN.MicroService.Internal.ApiGateway.Models;

namespace LINGYUN.MicroService.Internal.ApiGateway
{
    public class InternalApiGatewayOptions
    {
        public string AppId { get; set; }
        public DownstreamOpenApi[] DownstreamOpenApis { get; set; }
        public InternalApiGatewayOptions()
        {
            DownstreamOpenApis = new DownstreamOpenApi[0];
        }
    }
}
