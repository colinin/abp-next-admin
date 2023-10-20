namespace LINGYUN.Abp.Dapr.Client.DynamicProxying
{
    public class DaprClientProxy<TRemoteService> : IDaprClientProxy<TRemoteService>
    {
        public TRemoteService Service { get; }

        public DaprClientProxy(TRemoteService service)
        {
            Service = service;
        }
    }
}
