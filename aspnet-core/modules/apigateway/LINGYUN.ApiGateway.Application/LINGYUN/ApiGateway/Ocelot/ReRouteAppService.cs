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
        public async Task<ReRouteDto> CreateAsync(ReRouteCreateDto input)
        {
            var reRoute = ObjectMapper.Map<ReRouteCreateDto, ReRoute>(input);

            ApplyReRouteOptions(reRoute, input);

            reRoute = await _reRouteRepository.InsertAsync(reRoute, true);

            var reRouteDto = ObjectMapper.Map<ReRoute, ReRouteDto>(reRoute);

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(reRoute.AppId, "ReRoute", "Create"));

            return reRouteDto;
        }

        [Authorize(ApiGatewayPermissions.Route.Update)]
        public async Task<ReRouteDto> UpdateAsync(ReRouteUpdateDto input)
        {
            var reRoute = await _reRouteRepository.GetByReRouteIdAsync(long.Parse(input.ReRouteId));
            reRoute.SetRouteName(input.ReRouteName);
            reRoute.DangerousAcceptAnyServerCertificateValidator = input.DangerousAcceptAnyServerCertificateValidator;
            reRoute.DownstreamScheme = input.DownstreamScheme;
            reRoute.Key = input.Key;
            reRoute.Priority = input.Priority;
            reRoute.RequestIdKey = input.RequestIdKey;
            reRoute.ReRouteIsCaseSensitive = input.ReRouteIsCaseSensitive;
            reRoute.ServiceName = input.ServiceName;
            reRoute.ServiceNamespace = input.ServiceNamespace;
            reRoute.Timeout = input.Timeout;
            reRoute.UpstreamHost = input.UpstreamHost;
            reRoute.DownstreamHttpVersion = input.DownstreamHttpVersion;

            reRoute.SetDownstreamHeader(input.DownstreamHeaderTransform);
            reRoute.SetQueriesParamter(input.AddQueriesToRequest);
            reRoute.SetRequestClaims(input.AddClaimsToRequest);
            reRoute.SetRequestHeader(input.AddHeadersToRequest);
            reRoute.SetRouteClaims(input.RouteClaimsRequirement);
            reRoute.SetUpstreamHeader(input.UpstreamHeaderTransform);
            reRoute.SetChangeDownstreamPath(input.ChangeDownstreamPathTemplate);
            reRoute.SetDelegatingHandler(input.DelegatingHandlers);

            ApplyReRouteOptions(reRoute, input);

            reRoute = await _reRouteRepository.UpdateAsync(reRoute, true);

            var reRouteDto = ObjectMapper.Map<ReRoute, ReRouteDto>(reRoute);

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(reRoute.AppId, "ReRoute", "Modify"));

            return reRouteDto;
        }

        public async Task<ReRouteDto> GetAsync(ReRouteGetByIdInputDto input)
        {
            var reRoute = await _reRouteRepository.GetByReRouteIdAsync(input.RouteId);

            return ObjectMapper.Map<ReRoute, ReRouteDto>(reRoute);
        }

        public async Task<ReRouteDto> GetByRouteNameAsync(ReRouteGetByNameInputDto input)
        {
            var reRoute = await _reRouteRepository.GetByNameAsync(input.RouteName);

            return ObjectMapper.Map<ReRoute, ReRouteDto>(reRoute);
        }

        [Authorize(ApiGatewayPermissions.Route.Export)]
        public async Task<ListResultDto<ReRouteDto>> GetListByAppIdAsync(ReRouteGetByAppIdInputDto input)
        {
            await _routeGroupChecker.CheckActiveAsync(input.AppId);

            var routes = await _reRouteRepository.GetByAppIdAsync(input.AppId);

            return new ListResultDto<ReRouteDto>(ObjectMapper.Map<List<ReRoute>, List<ReRouteDto>>(routes));
        }

        public async Task<PagedResultDto<ReRouteDto>> GetListAsync(ReRouteGetByPagedInputDto input)
        {
            await _routeGroupChecker.CheckActiveAsync(input.AppId);
            var reroutesTuple = await _reRouteRepository
                .GetPagedListAsync(input.AppId, input.Filter, input.Sorting, 
                    input.SkipCount, input.MaxResultCount);

            return new PagedResultDto<ReRouteDto>(reroutesTuple.total, ObjectMapper.Map<List<ReRoute>, List<ReRouteDto>>(reroutesTuple.routes));
        }

        [Authorize(ApiGatewayPermissions.Route.Delete)]
        public async Task DeleteAsync(ReRouteGetByIdInputDto input)
        {
            var reRoute = await _reRouteRepository.GetByReRouteIdAsync(input.RouteId);

            await _reRouteRepository.DeleteAsync(reRoute);

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(reRoute.AppId, "ReRoute", "Delete"));
        }

        [Authorize(ApiGatewayPermissions.Route.Delete)]
        public async Task RemoveAsync(ReRouteGetByAppIdInputDto input)
        {
            await _routeGroupChecker.CheckActiveAsync(input.AppId);

            await _reRouteRepository.DeleteAsync(x => x.AppId.Equals(input.AppId));

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(input.AppId, "ReRoute", "Clean"));
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
