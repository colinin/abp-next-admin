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
        public async Task<ListResultDto<AggregateReRouteDto>> GetAsync(AggregateRouteGetByAppIdInputDto input)
        {
            return await AggregateReRouteAppService.GetAsync(input);
        }

        [HttpGet]
        public async Task<PagedResultDto<AggregateReRouteDto>> GetPagedListAsync(AggregateRouteGetByPagedInputDto input)
        {
            return await AggregateReRouteAppService.GetPagedListAsync(input);
        }

        [HttpGet]
        [Route("{RouteId}")]
        public async Task<AggregateReRouteDto> GetAsync(AggregateRouteGetByRouteIdInputDto input)
        {
            return await AggregateReRouteAppService.GetAsync(input);
        }

        [HttpPost]
        public async Task<AggregateReRouteDto> CreateAsync(AggregateReRouteCreateDto input)
        {
            return await AggregateReRouteAppService.CreateAsync(input);
        }

        [HttpPut]
        public async Task<AggregateReRouteDto> UpdateAsync(AggregateReRouteUpdateDto input)
        {
            return await AggregateReRouteAppService.UpdateAsync(input);
        }

        [HttpDelete]
        public async Task DeleteAsync(AggregateRouteGetByRouteIdInputDto input)
        {
            await AggregateReRouteAppService.DeleteAsync(input);
        }

        [HttpPost]
        [Route("RouteConfig")]
        public async Task<AggregateReRouteConfigDto> AddRouteConfigAsync(AggregateReRouteConfigCreateDto input)
        {
            return await AggregateReRouteAppService.AddRouteConfigAsync(input);
        }

        [HttpDelete]
        [Route("RouteConfig")]
        public async Task DeleteRouteConfigAsync(AggregateReRouteConfigGetByKeyInputDto input)
        {
            await AggregateReRouteAppService.DeleteRouteConfigAsync(input);
        }
    }
}
