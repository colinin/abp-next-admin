using System;
using System.Collections.Generic;
using System.Net.Http;

namespace LY.MicroService.ApiGateway
{
    public class InternalApiGatewayOptions
    {
        public Aggregator Aggregator { get; set; }

        public InternalApiGatewayOptions()
        {
            Aggregator = new Aggregator();
        }
    }

    public class Aggregator
    {
        /// <summary>
        /// 设置聚合路由
        /// </summary>
        /// <remarks>
        /// 聚合路由:
        /// /api/setting-management/settings
        /// </remarks>
        /// <example>
        /// 宿主端路由: 
        /// Get: /api/setting-management/settings/by-global
        /// Set: /api/setting-management/settings/change-global
        /// </example>
        /// <example>
        /// 租户端路由: 
        /// Get: /api/setting-management/settings/by-current-tenant
        /// Set: /api/setting-management/settings/change-current-tenant
        /// </example>
        /// <example>
        /// 用户端路由: 
        /// Get: /api/setting-management/settings/by-current-user
        /// Set: /api/setting-management/settings/change-current-user
        /// </example>
        public AggregatorUrl SettingUrl { get; set; }
        /// <summary>
        /// 应用配置聚合路由
        /// </summary>
        /// <remarks>
        /// 聚合路由:
        /// /api/abp/application-configuration
        /// </remarks>
        public AggregatorUrl ConfigurationUrl { get; set; }
        /// <summary>
        /// Api接口定义配置聚合路由
        /// </summary>
        /// <remarks>
        /// 聚合路由:
        /// /api/abp/api-definition
        /// </remarks>
        public AggregatorUrl ApiDefinitionUrl { get; set; }
        public Aggregator()
        {
            SettingUrl = new AggregatorUrl();
            ConfigurationUrl = new AggregatorUrl();
            ApiDefinitionUrl = new AggregatorUrl();
        }
    }

    public class AggregatorUrl
    {
        public string ClientName { get; set; }

        public HttpHandlerOptions HttpHandler { get; set; }
        /// <summary>
        /// 查询聚合路由列表
        /// </summary>
        public List<RequestUrl> GetUrls { get; set; }
        /// <summary>
        /// 变更路由
        /// </summary>
        public RequestUrl SetUrl { get; set; }
        /// <summary>
        /// 默认超时时间
        /// </summary>
        /// <remarks>
        /// default: 30s
        /// </remarks>
        public TimeSpan? DefaultTimeout { get; set; }

        public AggregatorUrl()
        {
            HttpHandler = new HttpHandlerOptions();
            GetUrls = new List<RequestUrl>();
            DefaultTimeout = TimeSpan.FromSeconds(30);
        }
    }

    public class RequestUrl
    {
        public HttpMethod Method { get; set; }
        public string Url { get; set; }
        public RequestUrl()
        {
            Method = HttpMethod.Get;
        }

        public RequestUrl(string url)
            : this(HttpMethod.Get, url)
        {
        }

        public RequestUrl(
            HttpMethod method,
            string url)
        {
            Method = method;
            Url = url;
        }
    }

    public class HttpHandlerOptions
    {
        public bool AllowAutoRedirect { get; set; }
        public bool UseCookieContainer { get; set; }
        public bool UseTracing { get; set; }
        public bool UseProxy { get; set; }
        public int MaxConnectionsPerServer { get; set; }
    }
}
