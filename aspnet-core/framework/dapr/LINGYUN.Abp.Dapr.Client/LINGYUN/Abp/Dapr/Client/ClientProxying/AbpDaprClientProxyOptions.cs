﻿using LINGYUN.Abp.Dapr.Client.DynamicProxying;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;

namespace LINGYUN.Abp.Dapr.Client.ClientProxying
{
    public class AbpDaprClientProxyOptions
    {
        public Dictionary<Type, DynamicDaprClientProxyConfig> DaprClientProxies { get; set; }
        /// <summary>
        /// 增加一个可配置的请求消息
        /// 参数一:    appId
        /// 参数二:    HttpRequestMessage
        /// </summary>
        public List<Action<string, HttpRequestMessage>> ProxyRequestActions { get; }
        /// <summary>
        /// 对响应进行处理,返回响应内容
        /// </summary>
        public Func<HttpResponseMessage, IAbpLazyServiceProvider, Task<string>> ProxyResponseContent { get; private set; }
        /// <summary>
        /// 格式化错误
        /// </summary>
        public Func<HttpResponseMessage, IAbpLazyServiceProvider, Task<RemoteServiceErrorInfo>> ProxyErrorFormat { get; private set; }
        public AbpDaprClientProxyOptions()
        {
            DaprClientProxies = new Dictionary<Type, DynamicDaprClientProxyConfig>();
            ProxyRequestActions = new List<Action<string, HttpRequestMessage>>();

            OnResponse(async (response, serviceProvider) =>
            {
                return await response.Content.ReadAsStringAsync();
            });
        }
        /// <summary>
        /// 处理服务间调用响应数据
        /// </summary>
        /// <param name="func"></param>
        public void OnResponse(Func<HttpResponseMessage, IAbpLazyServiceProvider, Task<string>> func)
        {
            ProxyResponseContent = func;
        }
        /// <summary>
        /// 处理服务间调用错误消息
        /// </summary>
        /// <param name="func"></param>
        public void OnError(Func<HttpResponseMessage, IAbpLazyServiceProvider, Task<RemoteServiceErrorInfo>> func)
        {
            ProxyErrorFormat = func;
        }
    }
}
