using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Dapr.Client
{
    public class DefaultDaprClientFactory : IDaprClientFactory
    {
        private readonly IOptionsMonitor<DaprClientFactoryOptions> _optionsMonitor;

        private readonly Func<string, Lazy<DaprClient>> _daprClientFactory;
        internal readonly ConcurrentDictionary<string, Lazy<DaprClient>> _daprClients;

        public DefaultDaprClientFactory(
            IOptionsMonitor<DaprClientFactoryOptions> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));

            _daprClients = new ConcurrentDictionary<string, Lazy<DaprClient>>();

            _daprClientFactory = (name) =>
            {
                return new Lazy<DaprClient>(() =>
                {
                    return InternalCreateDaprClient(name);
                }, LazyThreadSafetyMode.ExecutionAndPublication);
            };
        }

        public DaprClient CreateClient(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var client = _daprClients.GetOrAdd(name, _daprClientFactory).Value;

            var options = _optionsMonitor.Get(name);
            for (int i = 0; i < options.DaprClientActions.Count; i++)
            {
                options.DaprClientActions[i](client);
            }

            return client;
        }

        internal DaprClient InternalCreateDaprClient(string name)
        {
            var builder = new DaprClientBuilder();

            var options = _optionsMonitor.Get(name);

            if (!string.IsNullOrWhiteSpace(options.HttpEndpoint))
            {
                builder.UseHttpEndpoint(options.HttpEndpoint);
            }
            if (!string.IsNullOrWhiteSpace(options.GrpcEndpoint))
            {
                builder.UseGrpcEndpoint(options.GrpcEndpoint);
            }
            if (options.GrpcChannelOptions != null)
            {
                builder.UseGrpcChannelOptions(options.GrpcChannelOptions);
            }
            if (options.JsonSerializerOptions != null)
            {
                builder.UseJsonSerializationOptions(options.JsonSerializerOptions);
            }

            builder.UseDaprApiToken(options.DaprApiToken);

            for (int i = 0; i < options.DaprClientBuilderActions.Count; i++)
            {
                options.DaprClientBuilderActions[i](builder);
            }

            return builder.Build();
        }
    }
}
