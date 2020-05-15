using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Authorize(ApiGatewayPermissions.AggregateRoute.Default)]
    public class AggregateReRouteAppService : ApiGatewayApplicationServiceBase, IAggregateReRouteAppService
    {
        private readonly IAggregateReRouteRepository _aggregateReRouteRepository;

        public AggregateReRouteAppService(
            IAggregateReRouteRepository aggregateReRouteRepository)
        {
            _aggregateReRouteRepository = aggregateReRouteRepository;
        }

        [Authorize(ApiGatewayPermissions.AggregateRoute.Export)]
        public async Task<ListResultDto<AggregateReRouteDto>> GetAsync(AggregateRouteGetByAppIdInputDto aggregateRouteGetByAppId)
        {
            var reroutes = await _aggregateReRouteRepository.GetByAppIdAsync(aggregateRouteGetByAppId.AppId);

            return new ListResultDto<AggregateReRouteDto>(ObjectMapper.Map<List<AggregateReRoute>, List<AggregateReRouteDto>>(reroutes));
        }

        public async Task<PagedResultDto<AggregateReRouteDto>> GetPagedListAsync(AggregateRouteGetByPagedInputDto aggregateRouteGetByPaged)
        {
            var reroutesTuple = await _aggregateReRouteRepository
                .GetPagedListAsync(aggregateRouteGetByPaged.AppId, aggregateRouteGetByPaged.Filter, 
                                   aggregateRouteGetByPaged.Sorting, aggregateRouteGetByPaged.SkipCount, 
                                   aggregateRouteGetByPaged.MaxResultCount);

            return new PagedResultDto<AggregateReRouteDto>(reroutesTuple.total,
                ObjectMapper.Map<List<AggregateReRoute>, List<AggregateReRouteDto>>(reroutesTuple.routes));
        }
    }
}
