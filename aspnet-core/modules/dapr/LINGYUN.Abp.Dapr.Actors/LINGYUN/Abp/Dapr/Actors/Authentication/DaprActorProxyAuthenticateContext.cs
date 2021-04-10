using LINGYUN.Abp.Dapr.Actors.DynamicProxying;

namespace LINGYUN.Abp.Dapr.Actors.Authentication
{
    public class DaprActorProxyAuthenticateContext
    {
        public DaprHttpClientHandler Handler { get; }
        public DaprActorConfiguration RemoteService { get; }

        public string RemoteServiceName { get; }
        public DaprActorProxyAuthenticateContext(
            DaprHttpClientHandler handler,
            DaprActorConfiguration remoteService,
            string remoteServiceName)
        {
            Handler = handler;
            RemoteService = remoteService;
            RemoteServiceName = remoteServiceName;
        }
    }
}
