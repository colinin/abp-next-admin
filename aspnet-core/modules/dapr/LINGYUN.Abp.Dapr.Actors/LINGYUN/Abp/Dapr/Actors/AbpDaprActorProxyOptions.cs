using LINGYUN.Abp.Dapr.Actors.DynamicProxying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Dapr.Actors
{
    public class AbpDaprActorProxyOptions
    {
        public Dictionary<Type, DynamicDaprActorProxyConfig> ActorProxies { get; set; }

        public AbpDaprActorProxyOptions()
        {
            ActorProxies = new Dictionary<Type, DynamicDaprActorProxyConfig>();
        }
    }
}
