using Microsoft.Extensions.DependencyInjection;

namespace Dapr.Client
{
    public interface IDaprClientBuilder
    {
        string Name { get; }
        IServiceCollection Services { get; }
    }
}
