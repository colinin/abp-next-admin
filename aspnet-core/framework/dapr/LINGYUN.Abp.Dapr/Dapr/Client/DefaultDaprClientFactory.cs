using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading;
using Volo.Abp.Json.SystemTextJson;

namespace Dapr.Client
{
    public class DefaultDaprClientFactory : IDaprClientFactory
    {
        private readonly AbpSystemTextJsonSerializerOptions _systemTextJsonSerializerOptions;
        private readonly IOptionsMonitor<DaprClientFactoryOptions> _daprClientFactoryOptions;

        private readonly Func<string, Lazy<DaprClient>> _daprClientFactory;
        internal readonly ConcurrentDictionary<string, Lazy<DaprClient>> _daprClients;
        internal readonly ConcurrentDictionary<string, JsonSerializerOptions> _jsonSerializerOptions;

        public DefaultDaprClientFactory(
            IOptions<AbpSystemTextJsonSerializerOptions> systemTextJsonSerializerOptions,
            IOptionsMonitor<DaprClientFactoryOptions> daprClientFactoryOptions)
        {
            _daprClientFactoryOptions = daprClientFactoryOptions ?? throw new ArgumentNullException(nameof(daprClientFactoryOptions));
            _systemTextJsonSerializerOptions= systemTextJsonSerializerOptions?.Value ?? throw new ArgumentNullException(nameof(systemTextJsonSerializerOptions));

            _daprClients = new ConcurrentDictionary<string, Lazy<DaprClient>>();
            _jsonSerializerOptions = new ConcurrentDictionary<string, JsonSerializerOptions>();

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

            var options = _daprClientFactoryOptions.Get(name);
            for (var i = 0; i < options.DaprClientActions.Count; i++)
            {
                options.DaprClientActions[i](client);
            }

            return client;
        }

        internal DaprClient InternalCreateDaprClient(string name)
        {
            var builder = new DaprClientBuilder();

            var options = _daprClientFactoryOptions.Get(name);

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
            else
            {
                builder.UseJsonSerializationOptions(CreateJsonSerializerOptions(name));
            }

            builder.UseDaprApiToken(options.DaprApiToken);

            for (var i = 0; i < options.DaprClientBuilderActions.Count; i++)
            {
                options.DaprClientBuilderActions[i](builder);
            }

            return builder.Build();
        }

        private JsonSerializerOptions CreateJsonSerializerOptions(string name)
        {
            return _jsonSerializerOptions.GetOrAdd(name,
                _ => new JsonSerializerOptions(_systemTextJsonSerializerOptions.JsonSerializerOptions));
        }
    }
}
