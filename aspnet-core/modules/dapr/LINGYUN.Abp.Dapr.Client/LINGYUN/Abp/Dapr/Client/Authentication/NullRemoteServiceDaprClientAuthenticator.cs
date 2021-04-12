using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Dapr.Client.Authentication
{
    [Dependency(TryRegister = true)]
    public class NullRemoteServiceDaprClientAuthenticator : IRemoteServiceDaprClientAuthenticator, ISingletonDependency
    {
        public Task AuthenticateAsync(RemoteServiceDaprClientAuthenticateContext context)
        {
            return Task.CompletedTask;
        }
    }
}
