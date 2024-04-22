using System;

namespace LINGYUN.Abp.Dapr.Client.DynamicProxying
{
    public class DynamicDaprClientProxyConfig
    {
        public Type Type { get; }

        public string RemoteServiceName { get; }

        public DynamicDaprClientProxyConfig(Type type, string remoteServiceName)
        {
            Type = type;
            RemoteServiceName = remoteServiceName;
        }
    }
}
