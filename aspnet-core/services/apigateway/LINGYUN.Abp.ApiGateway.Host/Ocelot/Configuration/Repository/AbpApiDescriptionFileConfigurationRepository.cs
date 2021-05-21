using LINGYUN.Abp.ApiGateway;
using Microsoft.Extensions.Options;
using Ocelot.Configuration.File;
using Ocelot.Middleware.Multiplexer;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.DynamicProxying;
using Volo.Abp.Http.Modeling;

namespace Ocelot.Configuration.Repository
{
    /// <summary>
    /// 通过定义的RemoteServices来寻找abp端点定义的服务列表
    /// 在AbpApiGatewayOptions.AggrageRouteUrls中定义聚合的url
    /// 可以在发现相同端点后加入到聚合路由，聚合路由下游端点会自动加入聚合标识,以发现的聚合数量作为前缀
    /// 例如/api/abp/api-definition、/api/abp/application-configuration
    /// 下游端点将变成/aggregate1/api/abp/api-definition、/aggregate1/api/abp/application-configuration
    /// 上游服务直接调用/api/abp/api-definition、/api/abp/application-configuration即可唤起路由聚合
    /// </summary>
    public class AbpApiDescriptionFileConfigurationRepository : IFileConfigurationRepository
    {
        private readonly AbpRemoteServiceOptions _remoteServiceOptions;
        private readonly AbpApiGatewayOptions _apiGatewayOptions;

        private readonly IApiDescriptionFinder _apiDescriptionFinder;
        private readonly IHttpClientFactory _httpClientFactory;
        public AbpApiDescriptionFileConfigurationRepository(
            IHttpClientFactory httpClientFactory,
            IApiDescriptionFinder apiDescriptionFinder,
            IOptions<AbpApiGatewayOptions> apiGatewayOptions,
            IOptions<AbpRemoteServiceOptions> remoteServiceOptions
            )
        {
            _httpClientFactory = httpClientFactory;
            _apiDescriptionFinder = apiDescriptionFinder;

            _apiGatewayOptions = apiGatewayOptions.Value;
            _remoteServiceOptions = remoteServiceOptions.Value;
        }
        public async Task<Response<FileConfiguration>> Get()
        {
            var fileConfiguration = new FileConfiguration
            {
                GlobalConfiguration = _apiGatewayOptions.GlobalConfiguration
            };
            var apiDescriptionModels = await GetApiDescriptionsAsync();
            int foundAggrageRouteCount = 0;
            foreach (var apiDescriptionModel in apiDescriptionModels)
            {
                foreach (var moduleModel in apiDescriptionModel.Value.Modules)
                {
                    foreach (var controllerModel in moduleModel.Value.Controllers)
                    {
                        foreach (var actionModel in controllerModel.Value.Actions)
                        {
                            var action = actionModel.Value;
                            var downstreamUrl = action.Url.EnsureStartsWith('/');

                            // TODO: 多个相同的下游路由地址应组合为聚合路由

                            // TODO: 下游路由地址已添加
                            var route = fileConfiguration.Routes
                                .Where(route => HasBeenAddedRoute(route, downstreamUrl))
                                .LastOrDefault();
                            string aggregateKey = "";

                            if (route != null)
                            {
                                // TODO: 下游方法已添加
                                if (route.UpstreamHttpMethod.Any(method => method.Equals(action.HttpMethod, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    if (_apiGatewayOptions.AggrageRouteUrls.Any(url => url.Equals(downstreamUrl)))
                                    {
                                        foundAggrageRouteCount++;
                                        var aggregateRoute = fileConfiguration.Aggregates
                                            .Where(route => HasBeenAddedAggregateRoute(route, downstreamUrl))
                                            .FirstOrDefault();
                                        if (aggregateRoute == null)
                                        {
                                            aggregateRoute = new FileAggregateRoute
                                            {
                                                RouteIsCaseSensitive = false,
                                                // TODO: 可实现自定义的聚合提供装置
                                                Aggregator = nameof(AbpResponseMergeAggregator),
                                                UpstreamPathTemplate = downstreamUrl,
                                                RouteKeys = new List<string>()
                                            };
                                            fileConfiguration.Aggregates.Add(aggregateRoute);
                                        }
                                        if (route.Key.IsNullOrWhiteSpace())
                                        {
                                            route.Key = $"aggregate{foundAggrageRouteCount}";
                                            route.UpstreamPathTemplate = $"/{route.Key}{route.UpstreamPathTemplate}";
                                            aggregateRoute.RouteKeys.Add(route.Key);
                                            foundAggrageRouteCount++;
                                        }
                                        aggregateKey = $"aggregate{foundAggrageRouteCount}";
                                        aggregateRoute.RouteKeys.Add(aggregateKey);
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    route.UpstreamHttpMethod.Add(action.HttpMethod);
                                    continue;
                                }
                            }

                            var newRoute = new FileRoute
                            {
                                Key = aggregateKey,
                                UpstreamPathTemplate = (aggregateKey + downstreamUrl).EnsureStartsWith('/'),
                                DownstreamPathTemplate = downstreamUrl,
                                DangerousAcceptAnyServerCertificateValidator = false
                            };

                            var baseUrl = apiDescriptionModel.Key;
                            baseUrl = baseUrl.StartsWith("http://") ? baseUrl[7..] : baseUrl;
                            baseUrl = baseUrl.StartsWith("https://") ? baseUrl[8..] : baseUrl;
                            baseUrl = baseUrl.EndsWith("/") ? baseUrl[0..^1] : baseUrl;

                            var addresses = baseUrl.Split(":");
                            var hostAndPort = new FileHostAndPort
                            {
                                Host = addresses[0]
                            };
                            if (addresses.Length == 2)
                            {
                                hostAndPort.Port = int.Parse(addresses[1]);
                            }
                            newRoute.DownstreamHostAndPorts.Add(hostAndPort);
                            newRoute.UpstreamHttpMethod.Add(action.HttpMethod);

                            // abp api版本号支持通过query发送
                            //newRoute.DownstreamHttpVersion = action.SupportedVersions.Last();

                            fileConfiguration.Routes.Add(newRoute);
                        }
                    }
                }
            }

            return new OkResponse<FileConfiguration>(fileConfiguration);
        }

        private static bool HasBeenAddedRoute(FileRoute route, string downstreamUrl)
        {
            return route.DownstreamPathTemplate.Equals(downstreamUrl, StringComparison.CurrentCultureIgnoreCase);
        }

        private static bool HasBeenAddedAggregateRoute(FileAggregateRoute route, string upstreamUrl)
        {
            return route.UpstreamPathTemplate.Equals(upstreamUrl, StringComparison.CurrentCultureIgnoreCase);
        }

        public async Task<Response> Set(FileConfiguration fileConfiguration)
        {
            return await Task.FromResult(new OkResponse<FileConfiguration>(fileConfiguration));
        }

        protected virtual async Task<Dictionary<string, ApplicationApiDescriptionModel>> GetApiDescriptionsAsync()
        {
            var client = _httpClientFactory.CreateClient("_AbpApiDefinitionClient");

            var apiDescriptionModels = new Dictionary<string, ApplicationApiDescriptionModel>();

            foreach (var remoteService in _remoteServiceOptions.RemoteServices)
            {
                var model = await _apiDescriptionFinder.GetApiDescriptionAsync(client, remoteService.Value.BaseUrl);

                apiDescriptionModels.Add(remoteService.Value.BaseUrl, model);
            }

            return apiDescriptionModels;
        }
    }
}
