using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;

namespace LINGYUN.Abp.AI.Tools.Mcp;
public class McpAIToolProvider : IAIToolProvider, ITransientDependency
{
    public const string ProviderName = "Mcp";
    public string Name => ProviderName;

    protected IServiceProvider ServiceProvider { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    public McpAIToolProvider(
        IServiceProvider serviceProvider,
        IHttpClientFactory httpClientFactory)
    {
        ServiceProvider = serviceProvider;
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

        if (definition.IsUseMcpCurrentAccessToken())
        {
            var accessTokenProvider = ServiceProvider.GetRequiredService<IAbpAccessTokenProvider>();

            var token = await accessTokenProvider.GetTokenAsync();
            if (!token.IsNullOrWhiteSpace())
            {
                // TODO: 使用OAuth配置?
                httpClientTransportOptions.AdditionalHeaders.TryAdd("Authorization", $"Bearer {token}");
            }
        }

        var mcpClient = await McpClient.CreateAsync(
            new HttpClientTransport(httpClientTransportOptions, httpClient));

        return (await mcpClient.ListToolsAsync()).ToArray();
    }
}
