namespace LINGYUN.Abp.Dapr.Client
{
    public class AbpDaprRemoteServiceOptions
    {
        public DaprRemoteServiceConfigurationDictionary RemoteServices { get; set; }

        public AbpDaprRemoteServiceOptions()
        {
            RemoteServices = new DaprRemoteServiceConfigurationDictionary();
        }
    }
}
