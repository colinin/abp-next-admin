using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Dapr.Client
{
    public class DaprClientFactoryOptions
    {
        public string DaprApiToken{ get; set; }
        public string HttpEndpoint { get; set; }
        public string GrpcEndpoint { get; set; }
        public GrpcChannelOptions GrpcChannelOptions { get; set; }
        public JsonSerializerOptions JsonSerializerOptions { get; set; }
        public IList<Action<DaprClient>> DaprClientActions { get; } = new List<Action<DaprClient>>();
        public IList<Action<DaprClientBuilder>> DaprClientBuilderActions { get; } = new List<Action<DaprClientBuilder>>();
        public DaprClientFactoryOptions()
        {
        }
    }
}
