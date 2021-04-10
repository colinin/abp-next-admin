using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.Dapr.Actors
{
    public class DaprActorConfigurationDictionary : Dictionary<string, DaprActorConfiguration>
    {
        public const string DefaultName = "Default";

        public DaprActorConfiguration Default
        {
            get => this.GetOrDefault(DefaultName);
            set => this[DefaultName] = value;
        }

        public DaprActorConfiguration GetConfigurationOrDefault(string name)
        {
            return this.GetOrDefault(name)
                   ?? Default
                   ?? throw new AbpException($"Dapr service '{name}' was not found and there is no default configuration.");
        }
    }
}
