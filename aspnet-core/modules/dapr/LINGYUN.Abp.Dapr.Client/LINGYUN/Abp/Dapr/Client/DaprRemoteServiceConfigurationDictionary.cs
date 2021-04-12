using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.Dapr.Client
{
    public class DaprRemoteServiceConfigurationDictionary : Dictionary<string, DaprRemoteServiceConfiguration>
    {
        public const string DefaultName = "Default";

        public DaprRemoteServiceConfiguration Default
        {
            get => this.GetOrDefault(DefaultName);
            set => this[DefaultName] = value;
        }

        public DaprRemoteServiceConfiguration GetConfigurationOrDefault(string name)
        {
            return this.GetOrDefault(name)
                   ?? Default
                   ?? throw new AbpException($"Dapr remote service '{name}' was not found and there is no default configuration.");
        }
    }
}
