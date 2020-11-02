using LINGYUN.ApiGateway.EventBus;
using LINGYUN.ApiGateway.Snowflake;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.ApiGateway.Ocelot
{
    [Authorize(ApiGatewayPermissions.Global.Default)]
    public class GlobalConfigurationAppService : ApiGatewayApplicationServiceBase, IGlobalConfigurationAppService
    {
        private IDistributedEventBus _eventBus;
        protected IDistributedEventBus DistributedEventBus => LazyGetRequiredService(ref _eventBus);

        private readonly IRouteGroupChecker _routeGroupChecker;
        private readonly IGlobalConfigRepository _globalConfigRepository;
        private readonly ISnowflakeIdGenerator _snowflakeIdGenerator;
        public GlobalConfigurationAppService(
            IRouteGroupChecker routeGroupChecker,
            ISnowflakeIdGenerator snowflakeIdGenerator,
            IGlobalConfigRepository globalConfigRepository
            )
        {
            _routeGroupChecker = routeGroupChecker;
            _snowflakeIdGenerator = snowflakeIdGenerator;
            _globalConfigRepository = globalConfigRepository;
        }

        [Authorize(ApiGatewayPermissions.Global.Export)]
        public virtual async Task<GlobalConfigurationDto> GetAsync(GlobalGetByAppIdInputDto input)
        {
            await _routeGroupChecker.CheckActiveAsync(input.AppId);

            var globalConfig =  await _globalConfigRepository.GetByAppIdAsync(input.AppId);

            var globalConfigDto = ObjectMapper.Map<GlobalConfiguration, GlobalConfigurationDto>(globalConfig);

            return globalConfigDto;
        }

        [Authorize(ApiGatewayPermissions.Global.Create)]
        public virtual async Task<GlobalConfigurationDto> CreateAsync(GlobalCreateDto input)
        {
            await _routeGroupChecker.CheckActiveAsync(input.AppId);

            var globalConfiguration = new GlobalConfiguration(_snowflakeIdGenerator.NextId(), 
                input.BaseUrl, input.AppId);
            globalConfiguration.RequestIdKey = input.RequestIdKey;
            globalConfiguration.DownstreamScheme = input.DownstreamScheme;
            globalConfiguration.DownstreamHttpVersion = input.DownstreamHttpVersion;

            ApplyGlobalConfigurationOptions(globalConfiguration, input);

            globalConfiguration = await _globalConfigRepository.InsertAsync(globalConfiguration, true);

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(globalConfiguration.AppId, "Global", "Create"));

            return ObjectMapper.Map<GlobalConfiguration, GlobalConfigurationDto>(globalConfiguration);
        }

        [Authorize(ApiGatewayPermissions.Global.Update)]
        public async Task<GlobalConfigurationDto> UpdateAsync(GlobalUpdateDto input)
        {
            var globalConfiguration = await _globalConfigRepository.GetByItemIdAsync(input.ItemId);

            globalConfiguration.BaseUrl = input.BaseUrl;
            globalConfiguration.RequestIdKey = input.RequestIdKey;
            globalConfiguration.DownstreamScheme = input.DownstreamScheme;
            globalConfiguration.DownstreamHttpVersion = input.DownstreamHttpVersion;

            ApplyGlobalConfigurationOptions(globalConfiguration, input);

            globalConfiguration = await _globalConfigRepository.UpdateAsync(globalConfiguration, true);

            await DistributedEventBus.PublishAsync(new ApigatewayConfigChangeEventData(globalConfiguration.AppId, "Global", "Modify"));

            return ObjectMapper.Map<GlobalConfiguration, GlobalConfigurationDto>(globalConfiguration);
        }

        public virtual async Task<PagedResultDto<GlobalConfigurationDto>> GetAsync(GlobalGetByPagedInputDto input)
        {
            var globalsTupe = await _globalConfigRepository.GetPagedListAsync(input.Filter, input.Sorting,
                input.SkipCount, input.MaxResultCount);
            var globals = ObjectMapper.Map<List<GlobalConfiguration>, List<GlobalConfigurationDto>>(globalsTupe.Globals);

            return new PagedResultDto<GlobalConfigurationDto>(globalsTupe.TotalCount, globals);
        }

        [Authorize(ApiGatewayPermissions.Global.Delete)]
        public virtual async Task DeleteAsync(GlobalGetByAppIdInputDto input)
        {
            var globalConfiguration = await _globalConfigRepository.GetByAppIdAsync(input.AppId);
            await _globalConfigRepository.DeleteAsync(globalConfiguration);
        }

        private void ApplyGlobalConfigurationOptions(GlobalConfiguration globalConfiguration, GlobalConfigurationDtoBase configurationDto)
        {
            globalConfiguration.ServiceDiscoveryProvider.Type = configurationDto.ServiceDiscoveryProvider.Type;
            globalConfiguration.ServiceDiscoveryProvider.ConfigurationKey = configurationDto.ServiceDiscoveryProvider.ConfigurationKey;
            globalConfiguration.ServiceDiscoveryProvider.Namespace = configurationDto.ServiceDiscoveryProvider.Namespace;
            globalConfiguration.ServiceDiscoveryProvider.PollingInterval = configurationDto.ServiceDiscoveryProvider.PollingInterval;
            globalConfiguration.ServiceDiscoveryProvider.Scheme = configurationDto.ServiceDiscoveryProvider.Scheme;
            globalConfiguration.ServiceDiscoveryProvider.Token = configurationDto.ServiceDiscoveryProvider.Token; 
            globalConfiguration.ServiceDiscoveryProvider
                .BindServiceRegister(configurationDto.ServiceDiscoveryProvider.Host, configurationDto.ServiceDiscoveryProvider.Port);

            globalConfiguration.HttpHandlerOptions.ApplyAllowAutoRedirect(configurationDto.HttpHandlerOptions.AllowAutoRedirect);
            globalConfiguration.HttpHandlerOptions.ApplyCookieContainer(configurationDto.HttpHandlerOptions.UseCookieContainer);
            globalConfiguration.HttpHandlerOptions.ApplyHttpProxy(configurationDto.HttpHandlerOptions.UseProxy);
            globalConfiguration.HttpHandlerOptions.ApplyHttpTracing(configurationDto.HttpHandlerOptions.UseTracing);
            globalConfiguration.HttpHandlerOptions.SetMaxConnections(configurationDto.HttpHandlerOptions.MaxConnectionsPerServer);

            globalConfiguration.QoSOptions.ApplyQosOptions(configurationDto.QoSOptions.ExceptionsAllowedBeforeBreaking, configurationDto.QoSOptions.DurationOfBreak,
                configurationDto.QoSOptions.TimeoutValue);

            globalConfiguration.RateLimitOptions.SetLimitHeadersStatus(configurationDto.RateLimitOptions.DisableRateLimitHeaders);
            globalConfiguration.RateLimitOptions.ApplyRateLimitOptions(configurationDto.RateLimitOptions.ClientIdHeader, configurationDto.RateLimitOptions.QuotaExceededMessage,
                    configurationDto.RateLimitOptions.HttpStatusCode);

            globalConfiguration.LoadBalancerOptions.ApplyLoadBalancerOptions(configurationDto.LoadBalancerOptions.Type, configurationDto.LoadBalancerOptions.Key,
                configurationDto.LoadBalancerOptions.Expiry);
        }
    }
}
