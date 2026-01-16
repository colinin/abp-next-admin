using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.AI;
using OpenAI;
using System;
using System.ClientModel;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.AI;
public class OpenAIChatClientProvider : ChatClientProvider
{
    private const string DefaultEndpoint = "https://api.openai.com/v1";
    public const string ProviderName = "OpenAI";

    public override string Name => ProviderName;
    public OpenAIChatClientProvider(IServiceProvider serviceProvider) 
        : base(serviceProvider)
    {
    }

    public override Task<IChatClient> CreateAsync(WorkspaceDefinition workspace)
    {
        Check.NotNull(workspace, nameof(workspace));
        Check.NotNullOrWhiteSpace(workspace.ApiKey, nameof(WorkspaceDefinition.ApiKey));

        var openAIClient = new OpenAIClient(
            new ApiKeyCredential(workspace.ApiKey),
            new OpenAIClientOptions
            {
                Endpoint = new Uri(workspace.ApiBaseUrl ?? DefaultEndpoint),
            });

        var chatClient = openAIClient
            .GetChatClient(workspace.ModelName)
            .AsIChatClient()
            .AsBuilder()
            .UseLogging()
            .UseOpenTelemetry()
            .UseFunctionInvocation()
            .UseDistributedCache()
            .Build(ServiceProvider);

        return Task.FromResult(chatClient);
    }
}
