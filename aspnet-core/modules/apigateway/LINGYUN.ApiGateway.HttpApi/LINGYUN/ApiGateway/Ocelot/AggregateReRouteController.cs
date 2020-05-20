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

        [HttpGet]
        [Route("{RouteId}")]
        public async Task<AggregateReRouteDto> GetAsync(AggregateRouteGetByRouteIdInputDto aggregateRouteGetByRouteId)
        {
            return await AggregateReRouteAppService.GetAsync(aggregateRouteGetByRouteId);
        }

        [HttpPost]
        public async Task<AggregateReRouteDto> CreateAsync(AggregateReRouteCreateDto aggregateReRouteCreate)
        {
            return await AggregateReRouteAppService.CreateAsync(aggregateReRouteCreate);
        }

        [HttpPut]
        public async Task<AggregateReRouteDto> UpdateAsync(AggregateReRouteUpdateDto aggregateReRouteUpdate)
        {
            return await AggregateReRouteAppService.UpdateAsync(aggregateReRouteUpdate);
        }

        [HttpDelete]
        public async Task DeleteAsync(AggregateRouteGetByRouteIdInputDto aggregateRouteGetByRouteId)
        {
            await AggregateReRouteAppService.DeleteAsync(aggregateRouteGetByRouteId);
        }

        [HttpPost]
        [Route("RouteConfig")]
        public async Task<AggregateReRouteConfigDto> AddRouteConfigAsync(AggregateReRouteConfigCreateDto aggregateReRouteConfigCreate)
        {
            return await AggregateReRouteAppService.AddRouteConfigAsync(aggregateReRouteConfigCreate);
        }

        [HttpDelete]
        [Route("RouteConfig")]
        public async Task DeleteRouteConfigAsync(AggregateReRouteConfigGetByKeyInputDto aggregateReRouteConfigGetByKey)
        {
            await AggregateReRouteAppService.DeleteRouteConfigAsync(aggregateReRouteConfigGetByKey);
        }
    }
}
