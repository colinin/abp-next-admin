using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.ApiGateway.Ocelot
{
    [RemoteService(Name = ApiGatewayConsts.RemoteServiceName)]
    [Area("ApiGateway")]
    [Route("api/ApiGateway/Routes")]
    public class ReRouteController : ApiGatewayControllerBase, IReRouteAppService
    {
        protected IReRouteAppService ReRouteAppService { get; }
        public ReRouteController(
            IReRouteAppService reRouteAppService)
        {
            ReRouteAppService = reRouteAppService;
        }

        [HttpPost]
        public async Task<ReRouteDto> CreateAsync(ReRouteCreateDto routeCreateDto)
        {
            return await ReRouteAppService.CreateAsync(routeCreateDto);
        }

        [HttpPut]
        public async Task<ReRouteDto> UpdateAsync(ReRouteUpdateDto routeUpdateDto)
        {
            return await ReRouteAppService.UpdateAsync(routeUpdateDto);
        }

        [HttpGet]
        [Route("By-RouteId/{RouteId}")]
        public async Task<ReRouteDto> GetAsync(ReRouteGetByIdInputDto routeGetById)
        {
            return await ReRouteAppService.GetAsync(routeGetById);
        }

        [HttpGet]
        [Route("By-RouteName/{RouteName}")]
        public async Task<ReRouteDto> GetByRouteNameAsync(ReRouteGetByNameInputDto routeGetByName)
        {
            return await ReRouteAppService.GetByRouteNameAsync(routeGetByName);
        }

        [HttpGet]
        [Route("By-AppId/{AppId}")]
        public async Task<ListResultDto<ReRouteDto>> GetAsync(ReRouteGetByAppIdInputDto routeGetByAppId)
        {
            return await ReRouteAppService.GetAsync(routeGetByAppId);
        }

        [HttpGet]
        public async Task<PagedResultDto<ReRouteDto>> GetPagedListAsync(ReRouteGetByPagedInputDto routeGetByPaged)
        {
            return await ReRouteAppService.GetPagedListAsync(routeGetByPaged);
        }

        [HttpDelete]
        [Route("Clear")]
        public async Task RemoveAsync(ReRouteGetByAppIdInputDto routeGetByAppId)
        {
            await ReRouteAppService.RemoveAsync(routeGetByAppId);
        }

        [HttpDelete]
        public async Task DeleteAsync(ReRouteGetByIdInputDto routeGetById)
        {
            await ReRouteAppService.DeleteAsync(routeGetById);
        }
    }
}
