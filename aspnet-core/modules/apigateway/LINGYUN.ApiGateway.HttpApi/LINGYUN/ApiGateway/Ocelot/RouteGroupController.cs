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
        public virtual async Task<RouteGroupDto> CreateAsync(RouteGroupCreateDto routerCreateDto)
        {
            return await RouterAppService.CreateAsync(routerCreateDto);
        }

        [HttpGet]
        [Route("By-AppId/{AppId}")]
        public virtual async Task<RouteGroupDto> GetAsync(RouteGroupGetByAppIdInputDto routerGetByAppId)
        {
            return await RouterAppService.GetAsync(routerGetByAppId);
        }

        [HttpGet]
        [Route("Actived")]
        public virtual async Task<ListResultDto<RouteGroupAppIdsDto>> GetActivedAsync()
        {
            return await RouterAppService.GetActivedAsync();
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<RouteGroupDto>> GetAsync(RouteGroupGetByPagedInputDto routerGetByPagedInput)
        {
            return await RouterAppService.GetAsync(routerGetByPagedInput);
        }

        [HttpPut]
        public virtual async Task<RouteGroupDto> UpdateAsync(RouteGroupUpdateDto routerUpdateDto)
        {
            return await RouterAppService.UpdateAsync(routerUpdateDto);
        }

        [HttpDelete]
        public virtual async Task DeleteAsync(RouteGroupGetByAppIdInputDto routerGetByAppId)
        {
            await RouterAppService.DeleteAsync(routerGetByAppId);
        }
    }
}
