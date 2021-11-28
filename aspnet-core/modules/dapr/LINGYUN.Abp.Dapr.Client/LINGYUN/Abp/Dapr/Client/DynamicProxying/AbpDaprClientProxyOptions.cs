using System;
using System.Collections.Generic;
using System.Net.Http;

namespace LINGYUN.Abp.Dapr.Client.DynamicProxying
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

        public AbpDaprClientProxyOptions()
        {
            DaprClientProxies = new Dictionary<Type, DynamicDaprClientProxyConfig>();
            ProxyRequestActions = new List<Action<string, HttpRequestMessage>>();
        }
    }
}
