using System;

namespace LINGYUN.Abp.Dapr.Actors.DynamicProxying
{
    public class DynamicDaprActorProxyConfig
    {
        public Type Type { get; }

        public string RemoteServiceName { get; }

        public DynamicDaprActorProxyConfig(Type type, string remoteServiceName)
        {
            Type = type;
            RemoteServiceName = remoteServiceName;
        }
    }
}
