using Dapr.Client;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Dapr.Client
{
    public class AbpDaprClientBuilderOptions
    {
        public List<Action<string, DaprClient>> ProxyClientActions { get; }

        public List<Action<string, IServiceProvider, DaprClientBuilder>> ProxyClientBuildActions { get; }

        internal HashSet<string> ConfiguredProxyClients { get; }

        public AbpDaprClientBuilderOptions()
        {
            ConfiguredProxyClients = new HashSet<string>();
            ProxyClientActions = new List<Action<string, DaprClient>>();
            ProxyClientBuildActions = new List<Action<string, IServiceProvider, DaprClientBuilder>>();
        }
    }
}
