using LINGYUN.ApiGateWay.Admin.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;

namespace LINGYUN.ApiGateWay.Admin.Routes
{
    public class Route : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 应用标识
        /// </summary>
        public virtual string AppId { get; protected set; }
        /// <summary>
        /// 路由名称
        /// </summary>
        public virtual string Name { get; protected set; }
        /// <summary>
        /// 请求标识
        /// </summary>
        public virtual string RequestIdKey { get; set; }
        /// <summary>
        /// 是否区分大小写
        /// </summary>
        public virtual bool RouteIsCaseSensitive { get; set; }
        /// <summary>
        /// 服务名称
        /// </summary>
        public virtual string ServiceName { get; protected set; }
        /// <summary>
        /// 服务命名空间
        /// </summary>
        public virtual string ServiceNamespace { get; protected set; }
        /// <summary>
        /// 下游协议
        /// </summary>
        public virtual string DownstreamScheme { get; set; }
        /// <summary>
        /// 下游路由路径
        /// </summary>
        public virtual string DownstreamPathTemplate { get; protected set; }
        /// <summary>
        /// 上游路由路径
        /// </summary>
        public virtual string UpstreamPathTemplate { get; protected set; }
        /// <summary>
        /// 变更下游路径
        /// </summary>
        public virtual string ChangeDownstreamPathTemplate { get; protected set; }
        /// <summary>
        /// 上游主机
        /// </summary>
        public virtual string UpstreamHost { get; set; }
        /// <summary>
        /// 聚合标识
        /// </summary>
        public virtual string Key { get; set; }
        /// <summary>
        /// 优先级
        /// </summary>
        public virtual int? Priority { get; set; }
        /// <summary>
        /// 超时时间
        /// </summary>
        public virtual int? Timeout { get; set; }
        /// <summary>
        /// 忽略SSL警告
        /// </summary>
        public virtual bool DangerousAcceptAnyServerCertificateValidator { get; set; }
        /// <summary>
        /// 下游版本号
        /// </summary>
        public virtual string DownstreamHttpVersion { get; set; }
        /// <summary>
        /// 下游Http方法列表
        /// </summary>
        public virtual ICollection<RouteUpstreamHttpMethod> UpstreamHttpMethods { get; protected set; }
        /// <summary>
        /// 上游Http方法列表
        /// </summary>
        public virtual ICollection<RouteDownstreamHttpMethod> DownstreamHttpMethods { get; protected set; }
        /// <summary>
        /// 添加请求头参数
        /// </summary>
        public virtual ICollection<RouteAddRequestHttpHeader> AddHeadersToRequest { get; protected set; }
        /// <summary>
        /// 下游请求头转换
        /// </summary>
        public virtual ICollection<RouteUpstreamTransformHttpHeader> UpstreamHeaderTransform { get; protected set; }
        /// <summary>
        /// 上游请求头转换
        /// </summary>
        public virtual ICollection<RouteDownstreamTransformHttpHeader> DownstreamHeaderTransform { get; protected set; }
        /// <summary>
        /// 请求声明转换
        /// </summary>
        public virtual ICollection<RouteAddToRequestClaim> AddClaimsToRequest { get; protected set; }
        /// <summary>
        /// 请求必须的声明
        /// </summary>
        public virtual ICollection<RouteRequirementClaim> RouteClaimsRequirement { get; protected set; }
        /// <summary>
        /// 用户声明到请求头转换
        /// </summary>
        public virtual ICollection<RouteAddToRequestHttpQuery> AddQueriesToRequest { get; protected set; }
        /// <summary>
        /// 下游主机列表
        /// </summary>
        public virtual ICollection<RouteDownstreamHostAndPort> DownstreamHostAndPorts { get; protected set; }
        /// <summary>
        /// 授权处理器列表
        /// </summary>
        public virtual ICollection<RouteHttpDelegatingHandler> DelegatingHandlers { get; protected set; }
        /// <summary>
        /// Http选项
        /// </summary>
        public virtual RouteHttpHandler HttpHandler { get; protected set; }
        /// <summary>
        /// 授权
        /// </summary>
        public virtual RouteAuthentication Authentication { get; protected set; }
        /// <summary>
        /// 速率限制
        /// </summary>
        public virtual RouteRateLimitRule RouteRateLimitRule { get; protected set; }
        /// <summary>
        /// 负债均衡
        /// </summary>
        public virtual RouteLoadBalancer LoadBalancer { get; protected set; }
        /// <summary>
        /// 服务质量
        /// </summary>
        public virtual RouteQos QoS { get; protected set; }
        /// <summary>
        /// 文件缓存
        /// </summary>
        public virtual RouteCache Cache { get; protected set; }
        /// <summary>
        /// 安全策略
        /// </summary>
        public virtual RouteSecurity Security { get; protected set; }

        protected Route()
        {
            UpstreamHttpMethods = new Collection<RouteUpstreamHttpMethod>();
            DownstreamHttpMethods = new Collection<RouteDownstreamHttpMethod>();
            AddHeadersToRequest = new Collection<RouteAddRequestHttpHeader>();
            UpstreamHeaderTransform = new Collection<RouteUpstreamTransformHttpHeader>();
            DownstreamHeaderTransform = new Collection<RouteDownstreamTransformHttpHeader>();
            AddClaimsToRequest = new Collection<RouteAddToRequestClaim>();
            RouteClaimsRequirement = new Collection<RouteRequirementClaim>();
            AddQueriesToRequest = new Collection<RouteAddToRequestHttpQuery>();
            DownstreamHostAndPorts = new Collection<RouteDownstreamHostAndPort>();
            DelegatingHandlers = new Collection<RouteHttpDelegatingHandler>();
        }

        public void AddDownstreamHttpMethod(IGuidGenerator generator, string method)
        {
            DownstreamHttpMethods.AddIfNotContains(new RouteDownstreamHttpMethod(Id, generator.Create(), method)); ;
        }

        public void RemoveDownstreamHttpMethod(string method)
        {
            DownstreamHttpMethods.RemoveAll(x => x.Method.Equals(method, StringComparison.CurrentCultureIgnoreCase));
        }

        public void RemoveAllDownstreamHttpMethod()
        {
            DownstreamHttpMethods.Clear();
        }

        public void AddUpstreamHttpMethod(IGuidGenerator generator, string method)
        {
            UpstreamHttpMethods.AddIfNotContains(new RouteUpstreamHttpMethod(Id, generator.Create(), method));
        }

        public void RemoveUpstreamHttpMethod(string method)
        {
            UpstreamHttpMethods.RemoveAll(x => x.Method.Equals(method, StringComparison.CurrentCultureIgnoreCase));
        }

        public void RemoveAllUpstreamHttpMethod()
        {
            UpstreamHttpMethods.Clear();
        }

        public void AddRequestHeader(IGuidGenerator generator, string key, string value)
        {
            AddHeadersToRequest.AddIfNotContains(new RouteAddRequestHttpHeader(Id, generator.Create(), key, value));
        }

        public void RemoveRequestHeader(string key, string value)
        {
            AddHeadersToRequest.RemoveAll(x => x.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase)
                && x.Value.Equals(value, StringComparison.CurrentCultureIgnoreCase));
        }

        public void RemoveAllRequestHeader()
        {
            AddHeadersToRequest.Clear();
        }

        public void AddUpstreamHeaderTransform(IGuidGenerator generator, string key, string value)
        {
            UpstreamHeaderTransform.AddIfNotContains(new RouteUpstreamTransformHttpHeader(Id, generator.Create(), key, value));
        }

        public void RemoveUpstreamHeaderTransform(string key, string value)
        {
            UpstreamHeaderTransform.RemoveAll(x => x.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase)
                && x.Value.Equals(value, StringComparison.CurrentCultureIgnoreCase));
        }

        public void RemoveAllUpstreamHeaderTransform()
        {
            UpstreamHeaderTransform.Clear();
        }

        public void AddDownstreamHeaderTransform(IGuidGenerator generator, string key, string value)
        {
            DownstreamHeaderTransform.AddIfNotContains(new RouteDownstreamTransformHttpHeader(Id, generator.Create(), key, value));
        }

        public void RemoveDownstreamHeaderTransform(string key, string value)
        {
            DownstreamHeaderTransform.RemoveAll(x => x.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase)
                && x.Value.Equals(value, StringComparison.CurrentCultureIgnoreCase));
        }

        public void RemoveAllDownstreamHeaderTransform()
        {
            DownstreamHeaderTransform.Clear();
        }

        public void AddRequestClaim(IGuidGenerator generator, string key, string value)
        {
            AddClaimsToRequest.AddIfNotContains(new RouteAddToRequestClaim(Id, generator.Create(), key, value));
        }

        public void RemoveRequestClaim(string key, string value)
        {
            AddClaimsToRequest.RemoveAll(x => x.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase)
                && x.Value.Equals(value, StringComparison.CurrentCultureIgnoreCase));
        }

        public void RemoveAllRequestClaim()
        {
            AddClaimsToRequest.Clear();
        }

        public void AddRequirementClaim(IGuidGenerator generator, string key, string value)
        {
            RouteClaimsRequirement.AddIfNotContains(new RouteRequirementClaim(Id, generator.Create(), key, value));
        }

        public void RemoveRequirementClaim(string key, string value)
        {
            RouteClaimsRequirement.RemoveAll(x => x.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase)
                && x.Value.Equals(value, StringComparison.CurrentCultureIgnoreCase));
        }

        public void RemoveAllRequirementClaim()
        {
            RouteClaimsRequirement.Clear();
        }

        public void AddRequestQuery(IGuidGenerator generator, string key, string value)
        {
            AddQueriesToRequest.AddIfNotContains(new RouteAddToRequestHttpQuery(Id, generator.Create(), key, value));
        }

        public void RemoveRequestQuery(string key, string value)
        {
            AddQueriesToRequest.RemoveAll(x => x.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase)
                && x.Value.Equals(value, StringComparison.CurrentCultureIgnoreCase));
        }

        public void RemoveAllRequestQuery()
        {
            AddQueriesToRequest.Clear();
        }

        public void AddDownstreamHost(IGuidGenerator generator, string host, int? port)
        {
            DownstreamHostAndPorts.AddIfNotContains(new RouteDownstreamHostAndPort(Id, generator.Create(), host, port));
        }

        public void RemoveDownstreamHost(string host, int? port)
        {
            DownstreamHostAndPorts.RemoveAll(x => x.Host.Equals(host, StringComparison.CurrentCultureIgnoreCase)
                && x.Port.Equals(port));
        }

        public void RemoveAllDownstreamHost()
        {
            DownstreamHostAndPorts.Clear();
        }

        public void AddDelegatingHandler(IGuidGenerator generator, string name)
        {
            DelegatingHandlers.AddIfNotContains(new RouteHttpDelegatingHandler(Id, generator.Create(), name));
        }

        public void RemoveDelegatingHandler(string name)
        {
            DelegatingHandlers.RemoveAll(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public void RemoveAllDelegatingHandler()
        {
            DelegatingHandlers.Clear();
        }

        public void BindService(string serviceName, string @namespace)
        {
            ServiceName = serviceName;
            ServiceNamespace = @namespace;
        }

        public void UseHttpHandler(IGuidGenerator generator, bool autoRedirect = false, bool useCookie = false,
            bool useTracking = false, bool useProxy = false, int? maxConnection = 1000)
        {
            if (HttpHandler == null)
            {
                HttpHandler = new RouteHttpHandler(generator.Create(), Id);
            }
            HttpHandler.UseProxy = useProxy;
            HttpHandler.UseTracing = useTracking;
            HttpHandler.UseCookieContainer = useCookie;
            HttpHandler.AllowAutoRedirect = autoRedirect;
            HttpHandler.ChangeMaxConnection(maxConnection);
        }

        public void UseAuthentication(IGuidGenerator generator, string provider, IEnumerable<string> allowScope)
        {
            if (Authentication == null)
            {
                Authentication = new RouteAuthentication(generator.Create(), Id, provider);
            }
            Authentication.AddScopes(allowScope);
        }


    }
}
