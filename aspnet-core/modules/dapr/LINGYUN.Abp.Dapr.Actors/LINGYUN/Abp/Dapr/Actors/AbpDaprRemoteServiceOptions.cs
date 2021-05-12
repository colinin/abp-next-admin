using Volo.Abp.Http.Client;

namespace LINGYUN.Abp.Dapr.Actors
{
    public class AbpDaprRemoteServiceOptions
    {
        public RemoteServiceConfigurationDictionary RemoteServices { get; set; }

        public AbpDaprRemoteServiceOptions()
        {
            RemoteServices = new RemoteServiceConfigurationDictionary();
        }
    }
}
