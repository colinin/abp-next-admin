using LINGYUN.Abp.AI.Localization;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.Localization;

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

    public virtual AIToolPropertyDescriptor[] GetPropertites()
    {
        return [
            AIToolPropertyDescriptor.CreateStringProperty(
                McpAIToolDefinitionExtenssions.Endpoint,
                LocalizableString.Create<AbpAIResource>("McpAITool:Endpoint"),
                required: true),
            AIToolPropertyDescriptor.CreateDictionaryProperty(
                McpAIToolDefinitionExtenssions.Headers,
                LocalizableString.Create<AbpAIResource>("McpAITool:Headers")),
            AIToolPropertyDescriptor.CreateSelectProperty(
                McpAIToolDefinitionExtenssions.TransportMode,
                LocalizableString.Create<AbpAIResource>("McpAITool:TransportMode"),
                [
                    new("Auto Detect", HttpTransportMode.AutoDetect),
                    new("Streamable Http", HttpTransportMode.StreamableHttp),
                    new("Sse", HttpTransportMode.Sse),
                ],
                LocalizableString.Create<AbpAIResource>("McpAITool:TransportModeDesc")),
            AIToolPropertyDescriptor.CreateNumberProperty(
                McpAIToolDefinitionExtenssions.ConnectionTimeout,
                LocalizableString.Create<AbpAIResource>("McpAITool:ConnectionTimeout"),
                LocalizableString.Create<AbpAIResource>("McpAITool:ConnectionTimeoutDesc")),
            AIToolPropertyDescriptor.CreateNumberProperty(
                McpAIToolDefinitionExtenssions.MaxReconnectionAttempts,
                LocalizableString.Create<AbpAIResource>("McpAITool:MaxReconnectionAttempts"),
                LocalizableString.Create<AbpAIResource>("McpAITool:MaxReconnectionAttemptsDesc")),
            AIToolPropertyDescriptor.CreateBoolProperty(
                McpAIToolDefinitionExtenssions.CurrentAccessToken,
                LocalizableString.Create<AbpAIResource>("McpAITool:CurrentAccessToken"))];
    }

    public async virtual Task<AITool[]> CreateToolsAsync(AIToolDefinition definition)
    {
        try
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
        catch (Exception ex)
        {
            ServiceProvider
                .GetService<ILogger<McpAIToolProvider>>()
                ?.LogWarning(ex, "Mcp tool connection failed: {message}", ex.Message);
            return [];
        }
    }
}
