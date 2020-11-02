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
        public async Task<ReRouteDto> CreateAsync(ReRouteCreateDto input)
        {
            return await ReRouteAppService.CreateAsync(input);
        }

        [HttpPut]
        public async Task<ReRouteDto> UpdateAsync(ReRouteUpdateDto input)
        {
            return await ReRouteAppService.UpdateAsync(input);
        }

        [HttpGet]
        [Route("By-RouteId/{RouteId}")]
        public async Task<ReRouteDto> GetAsync(ReRouteGetByIdInputDto input)
        {
            return await ReRouteAppService.GetAsync(input);
        }

        [HttpGet]
        [Route("By-RouteName/{RouteName}")]
        public async Task<ReRouteDto> GetByRouteNameAsync(ReRouteGetByNameInputDto input)
        {
            return await ReRouteAppService.GetByRouteNameAsync(input);
        }

        [HttpGet]
        [Route("By-AppId/{AppId}")]
        public async Task<ListResultDto<ReRouteDto>> GetListByAppIdAsync(ReRouteGetByAppIdInputDto input)
        {
            return await ReRouteAppService.GetListByAppIdAsync(input);
        }

        [HttpGet]
        public async Task<PagedResultDto<ReRouteDto>> GetListAsync(ReRouteGetByPagedInputDto input)
        {
            return await ReRouteAppService.GetListAsync(input);
        }

        [HttpDelete]
        [Route("Clear")]
        public async Task RemoveAsync(ReRouteGetByAppIdInputDto input)
        {
            await ReRouteAppService.RemoveAsync(input);
        }

        [HttpDelete]
        public async Task DeleteAsync(ReRouteGetByIdInputDto input)
        {
            await ReRouteAppService.DeleteAsync(input);
        }
    }
}
