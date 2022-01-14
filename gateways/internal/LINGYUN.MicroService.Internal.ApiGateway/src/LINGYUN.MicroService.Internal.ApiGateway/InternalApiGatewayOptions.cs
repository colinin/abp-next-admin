using LINGYUN.MicroService.Internal.ApiGateway.Models;

namespace LINGYUN.MicroService.Internal.ApiGateway
{
    public class InternalApiGatewayOptions
    {
        /// <summary>
        /// 聚合异常时是否忽略
        /// </summary>
        public bool AggregationIgnoreError { get; set; }
        public DownstreamOpenApi[] DownstreamOpenApis { get; set; }
        public InternalApiGatewayOptions()
        {
            AggregationIgnoreError = true;
            DownstreamOpenApis = new DownstreamOpenApi[0];
        }
    }
}
