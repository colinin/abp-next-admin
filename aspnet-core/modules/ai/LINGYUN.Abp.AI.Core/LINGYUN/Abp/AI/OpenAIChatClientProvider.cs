using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.AI;
using OpenAI;
using System;
using System.ClientModel;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI;
public class OpenAIChatClientProvider : IChatClientProvider, ITransientDependency
{
    private const string DefaultEndpoint = "https://api.openai.com/v1";
    public const string ProviderName = "OpenAI";

    public virtual string Name => ProviderName;

    public virtual Task<IChatClient> CreateAsync(WorkspaceDefinition workspace)
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
            .AsIChatClient();

        return Task.FromResult(chatClient);
    }
}
