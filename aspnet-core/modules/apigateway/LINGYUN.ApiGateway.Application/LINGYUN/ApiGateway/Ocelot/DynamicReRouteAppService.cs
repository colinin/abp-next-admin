using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Authorize(ApiGatewayPermissions.DynamicRoute.Default)]
    public class DynamicReRouteAppService : ApiGatewayApplicationServiceBase, IDynamicReRouteAppService
    {
        private readonly IDynamicReRouteRepository _dynamicReRouteRepository;
        public DynamicReRouteAppService(IDynamicReRouteRepository dynamicReRouteRepository)
        {
            _dynamicReRouteRepository = dynamicReRouteRepository;
        }

        [Authorize(ApiGatewayPermissions.DynamicRoute.Export)]
        public async Task<ListResultDto<DynamicReRouteDto>> GetAsync(DynamicRouteGetByAppIdInputDto input)
        {
            var dynamicReRoutes = await _dynamicReRouteRepository.GetByAppIdAsync(input.AppId);

            return new ListResultDto<DynamicReRouteDto>(ObjectMapper.Map<List<DynamicReRoute>, List<DynamicReRouteDto>>(dynamicReRoutes));
        }
    }
}
