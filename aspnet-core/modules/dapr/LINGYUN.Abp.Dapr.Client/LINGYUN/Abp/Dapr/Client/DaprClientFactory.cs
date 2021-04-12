using Dapr.Client;
using Microsoft.Extensions.Options;
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json.SystemTextJson;

namespace LINGYUN.Abp.Dapr.Client
{
    public class DaprClientFactory : IDaprClientFactory, ISingletonDependency
    {
        protected AbpDaprClientOptions DaprClientOptions { get; }
        protected AbpSystemTextJsonSerializerOptions JsonSerializerOptions { get; }

        private readonly Lazy<DaprClient> _daprClientLazy;

        public DaprClientFactory(
            IOptions<AbpDaprClientOptions> daprClientOptions,
            IOptions<AbpSystemTextJsonSerializerOptions> jsonSerializarOptions)
        {
            DaprClientOptions = daprClientOptions.Value;
            JsonSerializerOptions = jsonSerializarOptions.Value;

            _daprClientLazy = new Lazy<DaprClient>(() => CreateDaprClient());
        }

        public DaprClient Create() => _daprClientLazy.Value;

        protected virtual DaprClient CreateDaprClient()
        {
            var builder = new DaprClientBuilder()
                .UseHttpEndpoint(DaprClientOptions.HttpEndpoint)
                .UseJsonSerializationOptions(JsonSerializerOptions.JsonSerializerOptions);

            if (!DaprClientOptions.GrpcEndpoint.IsNullOrWhiteSpace() && 
                DaprClientOptions.GrpcChannelOptions != null)
            {
                builder
                    .UseGrpcEndpoint(DaprClientOptions.GrpcEndpoint)
                    .UseGrpcChannelOptions(DaprClientOptions.GrpcChannelOptions);
            }

            return builder.Build();
        }
    }
}
