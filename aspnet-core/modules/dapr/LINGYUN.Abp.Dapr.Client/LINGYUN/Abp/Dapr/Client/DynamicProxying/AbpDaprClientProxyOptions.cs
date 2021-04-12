using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Dapr.Client.DynamicProxying
{
    public class AbpDaprClientProxyOptions
    {
        public Dictionary<Type, DynamicDaprClientProxyConfig> DaprClientProxies { get; set; }

        public AbpDaprClientProxyOptions()
        {
            DaprClientProxies = new Dictionary<Type, DynamicDaprClientProxyConfig>();
        }
    }
}
