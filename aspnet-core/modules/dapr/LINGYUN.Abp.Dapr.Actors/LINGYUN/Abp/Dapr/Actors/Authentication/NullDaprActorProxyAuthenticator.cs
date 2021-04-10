using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Dapr.Actors.Authentication
{
    [Dependency(TryRegister = true)]
    public class NullDaprActorProxyAuthenticator : IDaprActorProxyAuthenticator, ISingletonDependency
    {
        public Task AuthenticateAsync(DaprActorProxyAuthenticateContext context)
        {
            return Task.CompletedTask;
        }
    }
}
