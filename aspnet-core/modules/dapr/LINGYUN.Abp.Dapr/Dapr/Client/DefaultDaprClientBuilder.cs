using Microsoft.Extensions.DependencyInjection;

namespace Dapr.Client
{
    internal class DefaultDaprClientBuilder : IDaprClientBuilder
    {
        public DefaultDaprClientBuilder(IServiceCollection services, string name)
        {
            Services = services;
            Name = name;
        }

        public string Name { get; }

        public IServiceCollection Services { get; }
    }
}
