using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class ReRoute : AggregateRoot<int>
    {
        /// <summary>
        /// 路由ID
        /// </summary>
        public virtual long ReRouteId { get; private set; }
        /// <summary>
        /// 路由名称
        /// </summary>
        public virtual string ReRouteName { get; private set; }
        /// <summary>
        /// 下游路由路径
        /// </summary>
        public virtual string DownstreamPathTemplate { get; private set; }
        /// <summary>
        /// 变更下游路径
        /// </summary>
        public virtual string ChangeDownstreamPathTemplate { get; private set; }
        /// <summary>
        /// 下游请求方法
        /// </summary>
        public virtual string DownstreamHttpMethod { get; private set; }
        /// <summary>
        /// 上游路由路径
        /// </summary>
        public virtual string UpstreamPathTemplate { get; private set; }
        /// <summary>
        /// 下游Http方法列表,分号间隔
        /// </summary>
        public virtual string UpstreamHttpMethod { get; private set; }
        public virtual string AddHeadersToRequest { get; private set; }
        public virtual string UpstreamHeaderTransform { get; private set; }
        public virtual string DownstreamHeaderTransform { get; private set; }
        public virtual string AddClaimsToRequest { get; private set; }
        public virtual string RouteClaimsRequirement { get; private set; }
        public virtual string AddQueriesToRequest { get; private set; }
        public virtual string RequestIdKey { get; set; }
        public virtual CacheOptions CacheOptions { get; protected set; }
        public virtual bool ReRouteIsCaseSensitive { get; set; }
        public virtual string ServiceName { get; set; }
        public virtual string ServiceNamespace { get; set; }
        public virtual string DownstreamScheme { get; set; }
        public virtual QoSOptions QoSOptions { get; protected set; }
        public virtual LoadBalancerOptions LoadBalancerOptions { get; protected set; }
        public virtual RateLimitRule RateLimitOptions { get; protected set; }
        public virtual AuthenticationOptions AuthenticationOptions { get; protected set; }
        public virtual HttpHandlerOptions HttpHandlerOptions { get; protected set; }
        public virtual string DownstreamHostAndPorts { get; private set; }
        public virtual string DelegatingHandlers { get; private set; }
        public virtual string UpstreamHost { get; set; }
        public virtual string Key { get; set; }
        public virtual int? Priority { get; set; }
        public virtual int? Timeout { get; set; }
        public virtual bool DangerousAcceptAnyServerCertificateValidator { get; set; }
        public virtual SecurityOptions SecurityOptions { get; protected set; }
        public virtual string DownstreamHttpVersion { get; set; }
        public virtual string AppId { get; private set; }
        protected ReRoute()
        {

        }

        public ReRoute(long rerouteId, string routeName, string appId)
        {
            AppId = appId;
            ReRouteId = rerouteId;
            SetRouteName(routeName);
            InitlizaReRoute();
        }

        public void SetUpstream(string upPath, string upMethod)
        {
            UpstreamPathTemplate = upPath;
            UpstreamHttpMethod = upMethod;
        }

        public void SetDownstream(string downPath, string downHost, string downHttpMethod)
        {
            DownstreamPathTemplate = downPath;
            DownstreamHostAndPorts = downHost;
            DownstreamHttpMethod = downHttpMethod;
        }

        public void SetRouteName(string routeName)
        {
            ReRouteName = routeName;
        }

        public void SetDelegatingHandler(List<string> handlers)
        {
            DelegatingHandlers = string.Empty;
            foreach(var handler in handlers)
            {
                DelegatingHandlers += handler + ",";
            }
        }

        public void SetChangeDownstreamPath(Dictionary<string, string> downPaths)
        {
            ChangeDownstreamPathTemplate = string.Empty;
            foreach (var downPath in downPaths)
            {
                ChangeDownstreamPathTemplate += downPath.Key + ":" + downPath.Value;
                ChangeDownstreamPathTemplate += ",";
            }
        }

        public void SetQueriesParamter(Dictionary<string, string> paramters)
        {
            AddQueriesToRequest = string.Empty;
            foreach (var paramter in paramters)
            {
                AddQueriesToRequest += paramter.Key + ":" + paramter.Value;
                AddQueriesToRequest += ",";
            }
        }

        public void SetRouteClaims(Dictionary<string, string> claims)
        {
            RouteClaimsRequirement = string.Empty;
            foreach (var claim in claims)
            {
                RouteClaimsRequirement += claim.Key + ":" + claim.Value;
                RouteClaimsRequirement += ",";
            }
        }

        public void SetRequestClaims(Dictionary<string, string> claims)
        {
            AddClaimsToRequest = string.Empty;
            foreach (var claim in claims)
            {
                AddClaimsToRequest += claim.Key + ":" + claim.Value;
                AddClaimsToRequest += ",";
            }
        }

        public void SetDownstreamHeader(Dictionary<string, string> headers)
        {
            DownstreamHeaderTransform = string.Empty;
            foreach (var header in headers)
            {
                DownstreamHeaderTransform += header.Key + ":" + header.Value;
                DownstreamHeaderTransform += ",";
            }
        }

        public void SetUpstreamHeader(Dictionary<string, string> headers)
        {
            UpstreamHeaderTransform = string.Empty;
            foreach (var header in headers)
            {
                UpstreamHeaderTransform += header.Key + ":" + header.Value;
                UpstreamHeaderTransform += ",";
            }
        }

        public void SetRequestHeader(Dictionary<string, string> headers)
        {
            AddHeadersToRequest = string.Empty;
            foreach (var header in headers)
            {
                AddHeadersToRequest += header.Key + ":" + header.Value;
                AddHeadersToRequest += ",";
            }
        }

        private void InitlizaReRoute()
        {
            QoSOptions = new QoSOptions(null, null, 30000);
            QoSOptions.SetReRouteId(ReRouteId);
            CacheOptions = new CacheOptions(ReRouteId);
            LoadBalancerOptions = new LoadBalancerOptions("LeastConnection", "SessionId", null);
            LoadBalancerOptions.SetReRouteId(ReRouteId);
            RateLimitOptions = new RateLimitRule("", null, null);
            RateLimitOptions.SetReRouteId(ReRouteId);
            AuthenticationOptions = new AuthenticationOptions(ReRouteId);
            HttpHandlerOptions = HttpHandlerOptions.Default();
            HttpHandlerOptions.SetReRouteId(ReRouteId);
            SecurityOptions = new SecurityOptions(ReRouteId);
        }
    }
}
