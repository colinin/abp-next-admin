using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.ApiGateway.Ocelot
{
    [RemoteService(Name = ApiGatewayConsts.RemoteServiceName)]
    [Area("ApiGateway")]
    [Route("api/ApiGateway/RouteGroups")]
    public class RouteGroupController : ApiGatewayControllerBase, IRouteGroupAppService
    {
        protected IRouteGroupAppService RouterAppService { get; }
        public RouteGroupController(IRouteGroupAppService routerAppService)
        {
            RouterAppService = routerAppService;
        }

        [HttpPost]
        public virtual async Task<RouteGroupDto> CreateAsync(RouteGroupCreateDto input)
        {
            return await RouterAppService.CreateAsync(input);
        }

        [HttpGet]
        [Route("By-AppId/{AppId}")]
        public virtual async Task<RouteGroupDto> GetAsync(RouteGroupGetByAppIdInputDto input)
        {
            return await RouterAppService.GetAsync(input);
        }

        [HttpGet]
        [Route("Actived")]
        public virtual async Task<ListResultDto<RouteGroupAppIdsDto>> GetActivedAsync()
        {
            return await RouterAppService.GetActivedAsync();
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<RouteGroupDto>> GetAsync(RouteGroupGetByPagedInputDto input)
        {
            return await RouterAppService.GetAsync(input);
        }

        [HttpPut]
        public virtual async Task<RouteGroupDto> UpdateAsync(RouteGroupUpdateDto input)
        {
            return await RouterAppService.UpdateAsync(input);
        }

        [HttpDelete]
        public virtual async Task DeleteAsync(RouteGroupGetByAppIdInputDto input)
        {
            await RouterAppService.DeleteAsync(input);
        }
    }
}
