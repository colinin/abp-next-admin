using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.ApiGateway.Ocelot
{
    [RemoteService(Name = ApiGatewayConsts.RemoteServiceName)]
    [Area("ApiGateway")]
    [Route("api/ApiGateway/DynamicRoutes")]
    public class DynamicReRouteController : ApiGatewayControllerBase, IDynamicReRouteAppService
    {
        protected IDynamicReRouteAppService DynamicReRouteAppService { get; }
        public DynamicReRouteController(
            IDynamicReRouteAppService dynamicReRouteAppService)
        {
            DynamicReRouteAppService = dynamicReRouteAppService;
        }

        [HttpGet]
        [Route("By-AppId/{AppId}")]
        public virtual async Task<ListResultDto<DynamicReRouteDto>> GetAsync(DynamicRouteGetByAppIdInputDto input)
        {
            return await DynamicReRouteAppService.GetAsync(input);
        }
    }
}
