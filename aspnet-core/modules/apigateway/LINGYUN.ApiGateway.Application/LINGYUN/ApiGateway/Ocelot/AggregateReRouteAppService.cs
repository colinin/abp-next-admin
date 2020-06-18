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

        public virtual async Task<AggregateReRouteDto> GetAsync(AggregateRouteGetByRouteIdInputDto aggregateRouteGetByRouteId)
        {
            var routeId = long.Parse(aggregateRouteGetByRouteId.RouteId);
            var reroute = await _aggregateReRouteRepository.GetByRouteIdAsync(routeId);

            return ObjectMapper.Map<AggregateReRoute, AggregateReRouteDto>(reroute);
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

        [Authorize(ApiGatewayPermissions.AggregateRoute.Create)]
        public virtual async Task<AggregateReRouteDto> CreateAsync(AggregateReRouteCreateDto aggregateReRouteCreate)
        {
            var aggregateNameExists = await _aggregateReRouteRepository
                .AggregateReRouteNameExistsAsync(aggregateReRouteCreate.Name);
            if (aggregateNameExists)
            {
                throw new UserFriendlyException(L["AggregateReRouteExists", aggregateReRouteCreate.Name]);
            }
            var aggregateRoute = ObjectMapper.Map<AggregateReRouteCreateDto, AggregateReRoute>(aggregateReRouteCreate);
            aggregateRoute.SetUpstream(aggregateReRouteCreate.UpstreamHost, aggregateReRouteCreate.UpstreamPathTemplate);
            foreach (var httpMethod in aggregateReRouteCreate.UpstreamHttpMethod)
            {
                aggregateRoute.AddUpstreamHttpMethod(httpMethod);
            }
            foreach (var routeKey in aggregateReRouteCreate.ReRouteKeys)
            {
                aggregateRoute.AddRouteKey(routeKey);
            }
            aggregateRoute = await _aggregateReRouteRepository.InsertAsync(aggregateRoute);

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(aggregateRoute.AppId, "AggregateRoute", "Create"));

            return ObjectMapper.Map<AggregateReRoute, AggregateReRouteDto>(aggregateRoute);
        }

        [Authorize(ApiGatewayPermissions.AggregateRoute.Update)]
        public virtual async Task<AggregateReRouteDto> UpdateAsync(AggregateReRouteUpdateDto aggregateReRouteUpdate)
        {
            var routeId = long.Parse(aggregateReRouteUpdate.RouteId);
            var aggregateRoute = await _aggregateReRouteRepository.GetByRouteIdAsync(routeId);
            aggregateRoute.Priority = aggregateReRouteUpdate.Priority;
            aggregateRoute.ConcurrencyStamp = aggregateReRouteUpdate.ConcurrencyStamp;
            aggregateRoute.ReRouteIsCaseSensitive = aggregateReRouteUpdate.ReRouteIsCaseSensitive;
            aggregateRoute.Aggregator = aggregateReRouteUpdate.Aggregator;
            aggregateRoute.SetUpstream(aggregateReRouteUpdate.UpstreamHost, aggregateReRouteUpdate.UpstreamPathTemplate);

            aggregateRoute.RemoveAllUpstreamHttpMethod();
            foreach (var httpMethod in aggregateReRouteUpdate.UpstreamHttpMethod)
            {
                aggregateRoute.AddUpstreamHttpMethod(httpMethod);
            }

            aggregateRoute.RemoveAllRouteKey();
            foreach (var routeKey in aggregateReRouteUpdate.ReRouteKeys)
            {
                aggregateRoute.AddRouteKey(routeKey);
            }

            aggregateRoute = await _aggregateReRouteRepository.UpdateAsync(aggregateRoute, true);

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(aggregateRoute.AppId, "AggregateRoute", "Update"));

            return ObjectMapper.Map<AggregateReRoute, AggregateReRouteDto>(aggregateRoute);
        }

        [Authorize(ApiGatewayPermissions.AggregateRoute.Delete)]
        public virtual async Task DeleteAsync(AggregateRouteGetByRouteIdInputDto aggregateRouteGetByRouteId)
        {
            var routeId = long.Parse(aggregateRouteGetByRouteId.RouteId);
            var aggregateRoute = await _aggregateReRouteRepository.GetByRouteIdAsync(routeId);
            await _aggregateReRouteRepository.DeleteAsync(aggregateRoute);

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(aggregateRoute.AppId, "AggregateRoute", "Delete"));
        }

        [Authorize(ApiGatewayPermissions.AggregateRoute.ManageRouteConfig)]
        public virtual async Task<AggregateReRouteConfigDto> AddRouteConfigAsync(AggregateReRouteConfigCreateDto aggregateReRouteConfigCreate)
        {
            var routeId = long.Parse(aggregateReRouteConfigCreate.RouteId);
            var aggregateRoute = await _aggregateReRouteRepository.GetByRouteIdAsync(routeId);
            aggregateRoute.RemoveReRouteConfig(aggregateReRouteConfigCreate.ReRouteKey)
                .AddReRouteConfig(aggregateReRouteConfigCreate.ReRouteKey, aggregateReRouteConfigCreate.Parameter,
                    aggregateReRouteConfigCreate.JsonPath);
            var aggregateRouteConfig = aggregateRoute.FindReRouteConfig(aggregateReRouteConfigCreate.ReRouteKey);

            await _aggregateReRouteRepository.UpdateAsync(aggregateRoute);

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(aggregateRoute.AppId, "AggregateRoute", "AddRouteConfig"));

            return ObjectMapper.Map<AggregateReRouteConfig, AggregateReRouteConfigDto>(aggregateRouteConfig);
        }

        [Authorize(ApiGatewayPermissions.AggregateRoute.ManageRouteConfig)]
        public virtual async Task DeleteRouteConfigAsync(AggregateReRouteConfigGetByKeyInputDto aggregateReRouteConfigGetByKey)
        {
            var routeId = long.Parse(aggregateReRouteConfigGetByKey.RouteId);
            var aggregateRoute = await _aggregateReRouteRepository.GetByRouteIdAsync(routeId);
            aggregateRoute.RemoveReRouteConfig(aggregateReRouteConfigGetByKey.ReRouteKey);

            await _aggregateReRouteRepository.UpdateAsync(aggregateRoute);

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(aggregateRoute.AppId, "AggregateRoute", "DeleteRouteConfig"));
        }
    }
}
