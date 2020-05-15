using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.ApiGateway.Ocelot
{
    [RemoteService(Name = ApiGatewayConsts.RemoteServiceName)]
    [Area("ApiGateway")]
    [Route("api/ApiGateway/Aggregates")]
    public class AggregateReRouteController : ApiGatewayControllerBase, IAggregateReRouteAppService
    {
        protected IAggregateReRouteAppService AggregateReRouteAppService { get; }
        public AggregateReRouteController(
            IAggregateReRouteAppService aggregateReRouteAppService)
        {
            AggregateReRouteAppService = aggregateReRouteAppService;
        }

        [HttpGet]
        [Route("by-AppId/{AppId}")]
        public async Task<ListResultDto<AggregateReRouteDto>> GetAsync(AggregateRouteGetByAppIdInputDto aggregateRouteGetByAppId)
        {
            return await AggregateReRouteAppService.GetAsync(aggregateRouteGetByAppId);
        }

        [HttpGet]
        public async Task<PagedResultDto<AggregateReRouteDto>> GetPagedListAsync(AggregateRouteGetByPagedInputDto aggregateRouteGetByPaged)
        {
            return await AggregateReRouteAppService.GetPagedListAsync(aggregateRouteGetByPaged);
        }
    }
}
