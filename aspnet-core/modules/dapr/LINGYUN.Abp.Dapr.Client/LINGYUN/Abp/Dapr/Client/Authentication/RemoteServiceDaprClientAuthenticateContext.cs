using System.Net.Http;

namespace LINGYUN.Abp.Dapr.Client.Authentication
{
    public class RemoteServiceDaprClientAuthenticateContext
    {
        public HttpRequestMessage Request { get; }
        public DaprRemoteServiceConfiguration RemoteService { get; }

        public string RemoteServiceName { get; }
        public RemoteServiceDaprClientAuthenticateContext(
            HttpRequestMessage request,
            DaprRemoteServiceConfiguration remoteService,
            string remoteServiceName)
        {
            Request = request;
            RemoteService = remoteService;
            RemoteServiceName = remoteServiceName;
        }
    }
}
