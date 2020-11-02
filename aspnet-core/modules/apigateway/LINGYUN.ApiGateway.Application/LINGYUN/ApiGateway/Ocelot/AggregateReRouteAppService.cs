using LINGYUN.ApiGateway.EventBus;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.ApiGateway.Ocelot
{
    // 2020-05-20 15:00
    // TODO 重构项目 规范化实体的存储形式,数组类型分为多张表存储
    // TODO 取消long类型的实体主键 改用GUID

    [Authorize(ApiGatewayPermissions.AggregateRoute.Default)]
    public class AggregateReRouteAppService : ApiGatewayApplicationServiceBase, IAggregateReRouteAppService
    {
        private IDistributedEventBus _eventBus;
        protected IDistributedEventBus DistributedEventBus => LazyGetRequiredService(ref _eventBus);

        private readonly IAggregateReRouteRepository _aggregateReRouteRepository;

        public AggregateReRouteAppService(
            IAggregateReRouteRepository aggregateReRouteRepository)
        {
            _aggregateReRouteRepository = aggregateReRouteRepository;
        }

        public virtual async Task<AggregateReRouteDto> GetAsync(AggregateRouteGetByRouteIdInputDto input)
        {
            var routeId = long.Parse(input.RouteId);
            var reroute = await _aggregateReRouteRepository.GetByRouteIdAsync(routeId);

            return ObjectMapper.Map<AggregateReRoute, AggregateReRouteDto>(reroute);
        }

        [Authorize(ApiGatewayPermissions.AggregateRoute.Export)]
        public async Task<ListResultDto<AggregateReRouteDto>> GetAsync(AggregateRouteGetByAppIdInputDto input)
        {
            var reroutes = await _aggregateReRouteRepository.GetByAppIdAsync(input.AppId);

            return new ListResultDto<AggregateReRouteDto>(ObjectMapper.Map<List<AggregateReRoute>, List<AggregateReRouteDto>>(reroutes));
        }

        public async Task<PagedResultDto<AggregateReRouteDto>> GetPagedListAsync(AggregateRouteGetByPagedInputDto input)
        {
            var reroutesTuple = await _aggregateReRouteRepository
                .GetPagedListAsync(input.AppId, input.Filter, 
                                   input.Sorting, input.SkipCount, 
                                   input.MaxResultCount);

            return new PagedResultDto<AggregateReRouteDto>(reroutesTuple.total,
                ObjectMapper.Map<List<AggregateReRoute>, List<AggregateReRouteDto>>(reroutesTuple.routes));
        }

        [Authorize(ApiGatewayPermissions.AggregateRoute.Create)]
        public virtual async Task<AggregateReRouteDto> CreateAsync(AggregateReRouteCreateDto input)
        {
            var aggregateNameExists = await _aggregateReRouteRepository
                .AggregateReRouteNameExistsAsync(input.Name);
            if (aggregateNameExists)
            {
                throw new UserFriendlyException(L["AggregateReRouteExists", input.Name]);
            }
            var aggregateRoute = ObjectMapper.Map<AggregateReRouteCreateDto, AggregateReRoute>(input);
            aggregateRoute.SetUpstream(input.UpstreamHost, input.UpstreamPathTemplate);
            foreach (var httpMethod in input.UpstreamHttpMethod)
            {
                aggregateRoute.AddUpstreamHttpMethod(httpMethod);
            }
            foreach (var routeKey in input.ReRouteKeys)
            {
                aggregateRoute.AddRouteKey(routeKey);
            }
            aggregateRoute = await _aggregateReRouteRepository.InsertAsync(aggregateRoute);

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(aggregateRoute.AppId, "AggregateRoute", "Create"));

            return ObjectMapper.Map<AggregateReRoute, AggregateReRouteDto>(aggregateRoute);
        }

        [Authorize(ApiGatewayPermissions.AggregateRoute.Update)]
        public virtual async Task<AggregateReRouteDto> UpdateAsync(AggregateReRouteUpdateDto input)
        {
            var routeId = long.Parse(input.RouteId);
            var aggregateRoute = await _aggregateReRouteRepository.GetByRouteIdAsync(routeId);
            aggregateRoute.Priority = input.Priority;
            aggregateRoute.ConcurrencyStamp = input.ConcurrencyStamp;
            aggregateRoute.ReRouteIsCaseSensitive = input.ReRouteIsCaseSensitive;
            aggregateRoute.Aggregator = input.Aggregator;
            aggregateRoute.SetUpstream(input.UpstreamHost, input.UpstreamPathTemplate);

            aggregateRoute.RemoveAllUpstreamHttpMethod();
            foreach (var httpMethod in input.UpstreamHttpMethod)
            {
                aggregateRoute.AddUpstreamHttpMethod(httpMethod);
            }

            aggregateRoute.RemoveAllRouteKey();
            foreach (var routeKey in input.ReRouteKeys)
            {
                aggregateRoute.AddRouteKey(routeKey);
            }

            aggregateRoute = await _aggregateReRouteRepository.UpdateAsync(aggregateRoute, true);

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(aggregateRoute.AppId, "AggregateRoute", "Update"));

            return ObjectMapper.Map<AggregateReRoute, AggregateReRouteDto>(aggregateRoute);
        }

        [Authorize(ApiGatewayPermissions.AggregateRoute.Delete)]
        public virtual async Task DeleteAsync(AggregateRouteGetByRouteIdInputDto input)
        {
            var routeId = long.Parse(input.RouteId);
            var aggregateRoute = await _aggregateReRouteRepository.GetByRouteIdAsync(routeId);
            await _aggregateReRouteRepository.DeleteAsync(aggregateRoute);

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(aggregateRoute.AppId, "AggregateRoute", "Delete"));
        }

        [Authorize(ApiGatewayPermissions.AggregateRoute.ManageRouteConfig)]
        public virtual async Task<AggregateReRouteConfigDto> AddRouteConfigAsync(AggregateReRouteConfigCreateDto input)
        {
            var routeId = long.Parse(input.RouteId);
            var aggregateRoute = await _aggregateReRouteRepository.GetByRouteIdAsync(routeId);
            aggregateRoute.RemoveReRouteConfig(input.ReRouteKey)
                .AddReRouteConfig(input.ReRouteKey, input.Parameter,
                    input.JsonPath);
            var aggregateRouteConfig = aggregateRoute.FindReRouteConfig(input.ReRouteKey);

            await _aggregateReRouteRepository.UpdateAsync(aggregateRoute);

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(aggregateRoute.AppId, "AggregateRoute", "AddRouteConfig"));

            return ObjectMapper.Map<AggregateReRouteConfig, AggregateReRouteConfigDto>(aggregateRouteConfig);
        }

        [Authorize(ApiGatewayPermissions.AggregateRoute.ManageRouteConfig)]
        public virtual async Task DeleteRouteConfigAsync(AggregateReRouteConfigGetByKeyInputDto input)
        {
            var routeId = long.Parse(input.RouteId);
            var aggregateRoute = await _aggregateReRouteRepository.GetByRouteIdAsync(routeId);
            aggregateRoute.RemoveReRouteConfig(input.ReRouteKey);

            await _aggregateReRouteRepository.UpdateAsync(aggregateRoute);

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(aggregateRoute.AppId, "AggregateRoute", "DeleteRouteConfig"));
        }
    }
}
