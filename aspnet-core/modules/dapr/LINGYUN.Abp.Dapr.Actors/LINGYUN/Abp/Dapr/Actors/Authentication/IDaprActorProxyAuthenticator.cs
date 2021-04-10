using System.Threading.Tasks;

namespace LINGYUN.Abp.Dapr.Actors.Authentication
{
    public interface IDaprActorProxyAuthenticator
    {
        Task AuthenticateAsync(DaprActorProxyAuthenticateContext context);
    }
}
