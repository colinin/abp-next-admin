using System.Threading.Tasks;

namespace LINGYUN.Abp.Dapr.Client.Authentication
{
    public interface IRemoteServiceDaprClientAuthenticator
    {
        Task AuthenticateAsync(RemoteServiceDaprClientAuthenticateContext context);
    }
}
