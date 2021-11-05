using LINGYUN.ApiGateway.Models;

namespace LINGYUN.ApiGateway
{
    public class ApiGatewayOptions
    {
        public string AppId { get; set; }
        public DownstreamOpenApi[] DownstreamOpenApis { get; set; }
        public ApiGatewayOptions()
        {
            DownstreamOpenApis = new DownstreamOpenApi[0];
        }
    }
}
