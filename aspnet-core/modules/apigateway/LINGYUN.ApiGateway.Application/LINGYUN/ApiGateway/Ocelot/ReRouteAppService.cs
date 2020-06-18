using LINGYUN.ApiGateway.EventBus;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Authorize(ApiGatewayPermissions.Route.Default)]
    public class ReRouteAppService : ApiGatewayApplicationServiceBase, IReRouteAppService
    {
        private IDistributedEventBus _eventBus;
        protected IDistributedEventBus DistributedEventBus => LazyGetRequiredService(ref _eventBus);

        private readonly IRouteGroupChecker _routeGroupChecker;
        private readonly IReRouteRepository _reRouteRepository;

        public ReRouteAppService(
            IRouteGroupChecker routeGroupChecker,
            IReRouteRepository reRouteRepository
            )
        {
            _routeGroupChecker = routeGroupChecker;
            _reRouteRepository = reRouteRepository;
        }

        [Authorize(ApiGatewayPermissions.Route.Create)]
        public async Task<ReRouteDto> CreateAsync(ReRouteCreateDto routeCreateDto)
        {
            var reRoute = ObjectMapper.Map<ReRouteCreateDto, ReRoute>(routeCreateDto);

            ApplyReRouteOptions(reRoute, routeCreateDto);

            reRoute = await _reRouteRepository.InsertAsync(reRoute, true);

            var reRouteDto = ObjectMapper.Map<ReRoute, ReRouteDto>(reRoute);

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(reRoute.AppId, "ReRoute", "Create"));

            return reRouteDto;
        }

        [Authorize(ApiGatewayPermissions.Route.Update)]
        public async Task<ReRouteDto> UpdateAsync(ReRouteUpdateDto routeUpdateDto)
        {
            var reRoute = await _reRouteRepository.GetByReRouteIdAsync(long.Parse(routeUpdateDto.ReRouteId));
            reRoute.SetRouteName(routeUpdateDto.ReRouteName);
            reRoute.DangerousAcceptAnyServerCertificateValidator = routeUpdateDto.DangerousAcceptAnyServerCertificateValidator;
            reRoute.DownstreamScheme = routeUpdateDto.DownstreamScheme;
            reRoute.Key = routeUpdateDto.Key;
            reRoute.Priority = routeUpdateDto.Priority;
            reRoute.RequestIdKey = routeUpdateDto.RequestIdKey;
            reRoute.ReRouteIsCaseSensitive = routeUpdateDto.ReRouteIsCaseSensitive;
            reRoute.ServiceName = routeUpdateDto.ServiceName;
            reRoute.ServiceNamespace = routeUpdateDto.ServiceNamespace;
            reRoute.Timeout = routeUpdateDto.Timeout;
            reRoute.UpstreamHost = routeUpdateDto.UpstreamHost;
            reRoute.DownstreamHttpVersion = routeUpdateDto.DownstreamHttpVersion;

            reRoute.SetDownstreamHeader(routeUpdateDto.DownstreamHeaderTransform);
            reRoute.SetQueriesParamter(routeUpdateDto.AddQueriesToRequest);
            reRoute.SetRequestClaims(routeUpdateDto.AddClaimsToRequest);
            reRoute.SetRequestHeader(routeUpdateDto.AddHeadersToRequest);
            reRoute.SetRouteClaims(routeUpdateDto.RouteClaimsRequirement);
            reRoute.SetUpstreamHeader(routeUpdateDto.UpstreamHeaderTransform);
            reRoute.SetChangeDownstreamPath(routeUpdateDto.ChangeDownstreamPathTemplate);
            reRoute.SetDelegatingHandler(routeUpdateDto.DelegatingHandlers);

            ApplyReRouteOptions(reRoute, routeUpdateDto);

            reRoute = await _reRouteRepository.UpdateAsync(reRoute, true);

            var reRouteDto = ObjectMapper.Map<ReRoute, ReRouteDto>(reRoute);

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(reRoute.AppId, "ReRoute", "Modify"));

            return reRouteDto;
        }

        public async Task<ReRouteDto> GetAsync(ReRouteGetByIdInputDto routeGetById)
        {
            var reRoute = await _reRouteRepository.GetByReRouteIdAsync(routeGetById.RouteId);

            return ObjectMapper.Map<ReRoute, ReRouteDto>(reRoute);
        }

        public async Task<ReRouteDto> GetByRouteNameAsync(ReRouteGetByNameInputDto routeGetByName)
        {
            var reRoute = await _reRouteRepository.GetByNameAsync(routeGetByName.RouteName);

            return ObjectMapper.Map<ReRoute, ReRouteDto>(reRoute);
        }

        [Authorize(ApiGatewayPermissions.Route.Export)]
        public async Task<ListResultDto<ReRouteDto>> GetAsync(ReRouteGetByAppIdInputDto routeGetByAppId)
        {
            await _routeGroupChecker.CheckActiveAsync(routeGetByAppId.AppId);

            var routes = await _reRouteRepository.GetByAppIdAsync(routeGetByAppId.AppId);

            return new ListResultDto<ReRouteDto>(ObjectMapper.Map<List<ReRoute>, List<ReRouteDto>>(routes));
        }

        public async Task<PagedResultDto<ReRouteDto>> GetPagedListAsync(ReRouteGetByPagedInputDto routeGetByPaged)
        {
            await _routeGroupChecker.CheckActiveAsync(routeGetByPaged.AppId);
            var reroutesTuple = await _reRouteRepository
                .GetPagedListAsync(routeGetByPaged.AppId, routeGetByPaged.Filter, routeGetByPaged.Sorting, 
                    routeGetByPaged.SkipCount, routeGetByPaged.MaxResultCount);

            return new PagedResultDto<ReRouteDto>(reroutesTuple.total, ObjectMapper.Map<List<ReRoute>, List<ReRouteDto>>(reroutesTuple.routes));
        }

        [Authorize(ApiGatewayPermissions.Route.Delete)]
        public async Task DeleteAsync(ReRouteGetByIdInputDto routeGetById)
        {
            var reRoute = await _reRouteRepository.GetByReRouteIdAsync(routeGetById.RouteId);

            await _reRouteRepository.DeleteAsync(reRoute);

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(reRoute.AppId, "ReRoute", "Delete"));
        }

        [Authorize(ApiGatewayPermissions.Route.Delete)]
        public async Task RemoveAsync(ReRouteGetByAppIdInputDto routeGetByAppId)
        {
            await _routeGroupChecker.CheckActiveAsync(routeGetByAppId.AppId);

            await _reRouteRepository.DeleteAsync(x => x.AppId.Equals(routeGetByAppId.AppId));

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(routeGetByAppId.AppId, "ReRoute", "Clean"));
        }

        protected virtual void ApplyReRouteOptions(ReRoute reRoute, ReRouteDtoBase routeDto)
        {
            var downHostPorts = string.Empty;
            routeDto.DownstreamHostAndPorts.ForEach(hp => downHostPorts += hp.Host + ":" + hp.Port + ",");
            reRoute.SetDownstream(routeDto.DownstreamPathTemplate, downHostPorts, routeDto.DownstreamHttpMethod);

            var upHttpMethods = string.Empty;
            routeDto.UpstreamHttpMethod.ForEach(hm => upHttpMethods += hm + ",");
            reRoute.SetUpstream(routeDto.UpstreamPathTemplate, upHttpMethods);

            reRoute.AuthenticationOptions.ApplyAuthOptions(routeDto.AuthenticationOptions.AuthenticationProviderKey, routeDto.AuthenticationOptions.AllowedScopes);

            reRoute.CacheOptions.ApplyCacheOption(routeDto.FileCacheOptions.TtlSeconds, routeDto.FileCacheOptions.Region);

            reRoute.HttpHandlerOptions.ApplyAllowAutoRedirect(routeDto.HttpHandlerOptions.AllowAutoRedirect);
            reRoute.HttpHandlerOptions.ApplyCookieContainer(routeDto.HttpHandlerOptions.UseCookieContainer);
            reRoute.HttpHandlerOptions.ApplyHttpProxy(routeDto.HttpHandlerOptions.UseProxy);
            reRoute.HttpHandlerOptions.ApplyHttpTracing(routeDto.HttpHandlerOptions.UseTracing);
            reRoute.HttpHandlerOptions.SetMaxConnections(routeDto.HttpHandlerOptions.MaxConnectionsPerServer);

            reRoute.LoadBalancerOptions.ApplyLoadBalancerOptions(routeDto.LoadBalancerOptions.Type, routeDto.LoadBalancerOptions.Key, routeDto.LoadBalancerOptions.Expiry);

            reRoute.QoSOptions.ApplyQosOptions(routeDto.QoSOptions.ExceptionsAllowedBeforeBreaking, routeDto.QoSOptions.DurationOfBreak, routeDto.QoSOptions.TimeoutValue);

            reRoute.RateLimitOptions.ApplyRateLimit(routeDto.RateLimitOptions.EnableRateLimiting);
            reRoute.RateLimitOptions.SetPeriodTimespan(routeDto.RateLimitOptions.Period, routeDto.RateLimitOptions.PeriodTimespan, routeDto.RateLimitOptions.Limit);
            reRoute.RateLimitOptions.SetClientWhileList(routeDto.RateLimitOptions.ClientWhitelist);

            reRoute.SecurityOptions.SetAllowIpList(routeDto.SecurityOptions.IPAllowedList);
            reRoute.SecurityOptions.SetBlockIpList(routeDto.SecurityOptions.IPBlockedList);
        }
    }
}
