using LINGYUN.ApiGateway;
using LINGYUN.ApiGateway.Ocelot;
using Microsoft.Extensions.Options;
using Ocelot.Configuration.File;
using Ocelot.Responses;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;

namespace Ocelot.Configuration.Repository
{
    public class ApiHttpClientFileConfigurationRepository : IFileConfigurationRepository
    {
        private readonly IReRouteAppService _reRouteAppService;
        private readonly IGlobalConfigurationAppService _globalConfigurationAppService;
        private readonly IDynamicReRouteAppService _dynamicReRouteAppService;
        private readonly IAggregateReRouteAppService _aggregateReRouteAppService;
        private readonly IObjectMapper _objectMapper;

        protected ApiGatewayOptions ApiGatewayOptions { get; }
        public ApiHttpClientFileConfigurationRepository(
            IObjectMapper objectMapper,
            IOptions<ApiGatewayOptions> options,
            IReRouteAppService reRouteAppService,
            IDynamicReRouteAppService dynamicReRouteAppService,
            IAggregateReRouteAppService aggregateReRouteAppServicem,
            IGlobalConfigurationAppService globalConfigurationAppService
            )
        {
            _objectMapper = objectMapper;
            _reRouteAppService = reRouteAppService;
            _dynamicReRouteAppService = dynamicReRouteAppService;
            _aggregateReRouteAppService = aggregateReRouteAppServicem;
            _globalConfigurationAppService = globalConfigurationAppService;

            ApiGatewayOptions = options.Value;
        }
        public async Task<Response<FileConfiguration>> Get()
        {
            var fileConfiguration = new FileConfiguration();

            var globalConfiguration = await _globalConfigurationAppService.GetAsync(new GlobalGetByAppIdInputDto { AppId = ApiGatewayOptions.AppId });

            fileConfiguration.GlobalConfiguration = _objectMapper.Map<GlobalConfigurationDto, FileGlobalConfiguration>(globalConfiguration);

            var reRouteConfiguration = await _reRouteAppService.GetListByAppIdAsync(new ReRouteGetByAppIdInputDto { AppId = ApiGatewayOptions.AppId });

            if (reRouteConfiguration != null && reRouteConfiguration.Items.Count > 0)
            {
                foreach (var reRouteConfig in reRouteConfiguration.Items)
                {
                    fileConfiguration.Routes.Add(_objectMapper.Map<ReRouteDto, FileRoute>(reRouteConfig));
                }
            }

            var dynamicReRouteConfiguration = await _dynamicReRouteAppService.GetAsync(new DynamicRouteGetByAppIdInputDto { AppId = ApiGatewayOptions.AppId });

            if (dynamicReRouteConfiguration != null && dynamicReRouteConfiguration.Items.Count > 0)
            {
                foreach (var dynamicRouteConfig in dynamicReRouteConfiguration.Items)
                {
                    fileConfiguration.DynamicRoutes.Add(_objectMapper.Map<DynamicReRouteDto, FileDynamicRoute>(dynamicRouteConfig));
                }
            }

            var aggregateReRouteConfiguration = await _aggregateReRouteAppService.GetAsync(new AggregateRouteGetByAppIdInputDto { AppId = ApiGatewayOptions.AppId });
            if (aggregateReRouteConfiguration != null && aggregateReRouteConfiguration.Items.Count > 0)
            {
                foreach (var aggregateRouteConfig in aggregateReRouteConfiguration.Items)
                {
                    fileConfiguration.Aggregates.Add(_objectMapper.Map<AggregateReRouteDto, FileAggregateRoute>(aggregateRouteConfig));
                }
            }

            return new OkResponse<FileConfiguration>(fileConfiguration);
        }

        public async Task<Response> Set(FileConfiguration fileConfiguration)
        {
            // 不实现,从自己的微服务中去实现
            return await Task.FromResult(new OkResponse<FileConfiguration>(fileConfiguration));
        }
    }
}
