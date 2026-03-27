using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Tools.Mcp;
public class McpAIToolProvider : IAIToolProvider, ITransientDependency
{
    public const string ProviderName = "Mcp";
    public string Name => ProviderName;

    protected IHttpClientFactory HttpClientFactory { get; }
    public McpAIToolProvider(IHttpClientFactory httpClientFactory)
    {
        HttpClientFactory = httpClientFactory;
    }

    public async virtual Task<AITool[]> CreateToolsAsync(AIToolDefinition definition)
    {
        var httpClient = HttpClientFactory.CreateMcpAIToolClient();
        var httpClientTransportOptions = new HttpClientTransportOptions
        {
            Endpoint = new Uri(definition.GetMcpEndpoint()),
            AdditionalHeaders = new Dictionary<string, string>(),
            TransportMode = definition.GetMcpTransportMode(),
            ConnectionTimeout = definition.GetMcpConnectionTimeout(),
            MaxReconnectionAttempts = definition.GetMcpMaxReconnectionAttempts(),
        };

        var headers = definition.GetMcpHeaders();
        foreach (var header in headers)
        {
            httpClientTransportOptions.AdditionalHeaders.TryAdd(header.Key, header.Value);
        }

        var mcpClient = await McpClient.CreateAsync(
            new HttpClientTransport(httpClientTransportOptions, httpClient));

        return (await mcpClient.ListToolsAsync()).ToArray();
    }
}
