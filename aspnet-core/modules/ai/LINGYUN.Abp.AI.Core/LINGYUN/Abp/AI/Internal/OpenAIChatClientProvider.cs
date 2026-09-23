using LINGYUN.Abp.AI.Models;
using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI;
using System;
using System.ClientModel;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.AI.Internal;
public class OpenAIChatClientProvider : ChatClientProvider
{
    protected virtual string DefaultEndpoint => "https://api.openai.com/v1";

    public const string ProviderName = "OpenAI";
    public override string Name => ProviderName;
    public OpenAIChatClientProvider(IServiceProvider serviceProvider) 
        : base(serviceProvider)
    {
    }

    public override ChatModel[] GetModels()
    {
        return [
            new ChatModel("gpt-5.4", "gpt-5.4"),
            new ChatModel("gpt-5.4-mini", "gpt-5.4-mini"),
            new ChatModel("gpt-5.4-nano", "gpt-5.4-nano"),
            new ChatModel("gpt-5.5", "gpt-5.5"),
            new ChatModel("gpt-5.5-pro", "gpt-5.5-pro"),
        ];
    }

    public override async Task<IChatClient> CreateAsync(WorkspaceDefinition workspace)
    {
        Check.NotNull(workspace, nameof(workspace));
        Check.NotNullOrWhiteSpace(workspace.ApiKey, nameof(WorkspaceDefinition.ApiKey));

        var openAIClient = new OpenAIClient(
            new ApiKeyCredential(workspace.ApiKey),
            new OpenAIClientOptions
            {
                Endpoint = new Uri(workspace.ApiBaseUrl ?? DefaultEndpoint),
            });

        var options = ServiceProvider.GetRequiredService<IOptions<AbpAICoreOptions>>().Value;

        var chatClientBuilder = openAIClient
            .GetChatClient(workspace.ModelName)
            .AsIChatClient()
            .AsBuilder();

        foreach (var handlerAction in options.ChatClientBuildActions)
        {
            chatClientBuilder = await handlerAction(workspace, ServiceProvider, chatClientBuilder);
        }

        return chatClientBuilder.Build(ServiceProvider);
    }
}
